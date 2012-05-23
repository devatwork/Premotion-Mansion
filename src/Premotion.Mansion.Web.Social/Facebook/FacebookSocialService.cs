using System;
using System.Net;
using Premotion.Mansion.Core.Conversion;
using RestSharp;

namespace Premotion.Mansion.Web.Social.Facebook
{
	/// <summary>
	/// Implements the <see cref="ISocialService"/> for Facebook.
	/// </summary>
	public class FacebookSocialService : SocialServiceBase
	{
		#region Constants
		private const string OAathDialogEndpoint = "https://www.facebook.com/dialog/oauth";
		private const string GraphApiEndpoint = "https://graph.facebook.com";
		private const string ProfilePermissions = "email";
		#endregion
		#region Constructors
		/// <summary>
		/// Construct the Facebook social service.
		/// </summary>
		/// <param name="clientId">The client ID.</param>
		/// <param name="clientSecret">The client secret.</param>
		/// <param name="conversionService">The <see cref="IConversionService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="clientId"/> or <paramref name="clientSecret"/> is null.</exception>
		public FacebookSocialService(string clientId, string clientSecret, IConversionService conversionService) : base(Constants.ProviderName, conversionService)
		{
			// validate arguments
			if (string.IsNullOrEmpty(clientId))
				throw new ArgumentNullException("clientId");
			if (string.IsNullOrEmpty(clientSecret))
				throw new ArgumentNullException("clientSecret");

			// set values
			this.clientId = clientId;
			this.clientSecret = clientSecret;
		}
		#endregion
		#region Implementation of SocialServiceBase
		/// <summary>
		/// Retrieves the <see cref="Profile"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the <see cref="Result{Profile}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		protected override Result<Profile> DoRetrieveProfile(IMansionWebContext context)
		{
			return ExecuteRequest(context,
			                      client =>
			                      {
			                      	// create the request
			                      	var request = new RestRequest("me");

			                      	// execute the request
			                      	return client.Get<FacebookProfile>(request);
			                      },
			                      model => model.Map(),
			                      ProfilePermissions
				);
		}
		/// <summary>
		/// Exchanges the OAuth code for an access token.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="requestUri">The <see cref="Uri"/> of the current request, which usually contains the result of the OAuth workflow.</param>
		/// <returns>Returns the <see cref="Uri"/> of the request before starting the OAuth workflow.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		protected override Result<Uri> DoExchangeCodeForAccessToken(IMansionWebContext context, Uri requestUri)
		{
			// parse the query string
			var getParameters = requestUri.Query.ParseQueryString();

			// extract the state and possible error codes
			var state = getParameters.Get<Uri>(context, "state");
			var errorReason = getParameters.Get<string>(context, "error_reason");
			var error = getParameters.Get<string>(context, "error");
			var errorDescription = getParameters.Get<string>(context, "error_description");

			// check for errors
			if (!string.IsNullOrEmpty(errorReason) || !string.IsNullOrEmpty(error) || !string.IsNullOrEmpty(errorDescription))
			{
				// authentication failed because user clicked cancel,
				// redirect back with reason
				var modifiableUri = new UriBuilder(state);
				var queryString = modifiableUri.Query.ParseQueryString();
				queryString.Set("status", "failed");
				queryString.Set("reason", "cancelled-by-user");
				modifiableUri.Query = queryString.ToHttpSafeString();
				return Result<Uri>.Redirect(modifiableUri.Uri);
			}

			// get the code and state
			var code = getParameters.Get<string>(context, "code");

			// creat the client
			var client = new RestClient(GraphApiEndpoint);

			// create the request
			var request = new RestRequest("oauth/access_token")
				.AddParameter("client_id", clientId)
				.AddParameter("redirect_uri", BuildExchangeTokenRedirectUri(context).ToString())
				.AddParameter("client_secret", clientSecret)
				.AddParameter("code", code);

			// execute the request
			var response = client.Get(request);

			// TODO: add error checking

			// extract the access_token from the request
			var accessToken = response.Content.ParseQueryString().Get<string>(context, "access_token");

			// store the access token
			SetOAuthVariable(context, "accessToken", accessToken);

			// return the state, which is the url of the page before authentication took place
			return Result<Uri>.Success(state);
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Executes an request, handles.
		/// </summary>
		/// <typeparam name="TRawModel">The result returned from Facebook.</typeparam>
		/// <typeparam name="TModel">The model required by the return value.</typeparam>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="requestExecutor">The request executor.</param>
		/// <param name="mapper">The mapper.</param>
		/// <param name="scope">The required permission of the request.</param>
		/// <returns>Returns a <see cref="Result{TModel}"/></returns>
		private Result<TModel> ExecuteRequest<TRawModel, TModel>(IMansionWebContext context, Func<IRestClient, IRestResponse<TRawModel>> requestExecutor, Func<TRawModel, TModel> mapper, string scope = null) where TModel : class where TRawModel : ModelBase
		{
			// creat the client
			var client = new RestClient(GraphApiEndpoint);

			// check if an access token if not available, otherwise add it to the request
			string accessToken;
			if (!TryGetOAuthVariable(context, "accessToken", out accessToken))
			{
				//  start the OAuth workflow
				var redirectUri = BuildRedirectToFacebookDialogUri(context, scope);

				// return the result
				return Result<TModel>.Redirect(redirectUri);
			}
			client.AddDefaultParameter("access_token", accessToken);

			// execute the request
			var response = requestExecutor(client);

			// error handling here
			if (response.StatusCode == HttpStatusCode.BadRequest)
			{
				if ("OAuthException".Equals(response.Data.Error.Type, StringComparison.OrdinalIgnoreCase))
				{
					//  start the OAuth workflow
					var redirectUri = BuildRedirectToFacebookDialogUri(context, scope);

					// return the result
					return Result<TModel>.Redirect(redirectUri);
				}

				// unknown error
				throw new InvalidOperationException(string.Format("Facebook generated error {0} of type '{1}' with message '{2}'", response.Data.Error.Code, response.Data.Error.Type, response.Data.Error.Message));
			}

			// execute the request
			return Result<TModel>.Success(mapper(response.Data));
		}
		/// <summary>
		/// Builds the Facebook dialg <see cref="Uri"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="scope"></param>
		/// <returns></returns>
		private Uri BuildRedirectToFacebookDialogUri(IMansionWebContext context, string scope = null)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// state
			var state = context.HttpContext.Request.Url;
			if (state == null)
				throw new InvalidOperationException("Must have a state");

			// construct the client
			var client = new RestClient(OAathDialogEndpoint);

			// create the request
			var request = new RestRequest()
				.AddParameter("client_id", clientId)
				.AddParameter("redirect_uri", BuildExchangeTokenRedirectUri(context).ToString())
				.AddParameter("state", state.ToString());

			// add the scope when there is one
			if (scope != null)
				request.AddParameter("scope", scope);

			// get the uri of the request
			return client.BuildUri(request);
		}
		#endregion
		#region Private Fields
		private readonly string clientId;
		private readonly string clientSecret;
		#endregion
	}
}