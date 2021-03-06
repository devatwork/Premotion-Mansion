﻿using System;
using System.Configuration;
using System.Net;
using Premotion.Mansion.Core.Conversion;
using RestSharp;

namespace Premotion.Mansion.Web.Social.Facebook
{
	/// <summary>
	/// Implements the <see cref="ISocialService"/> for Facebook.
	/// </summary>
	public class FacebookSocialService : SocialServiceBase, IFacebookSocialService
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
		/// <param name="conversionService">The <see cref="IConversionService"/>.</param>
		public FacebookSocialService(IConversionService conversionService) : base(Constants.ProviderName, conversionService)
		{
			// set values
			var appSettings = ConfigurationManager.AppSettings;
			clientId = appSettings[Constants.AppIdApplicationSettingKey];
			clientSecret = appSettings[Constants.AppSecretApplicationSettingKey];
			if (string.IsNullOrEmpty(clientId))
				throw new InvalidOperationException("No valid value found in application settings for key " + Constants.AppIdApplicationSettingKey);
			if (string.IsNullOrEmpty(clientSecret))
				throw new InvalidOperationException("No valid value found in application settings for key " + Constants.AppSecretApplicationSettingKey);
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
		/// <param name="requestUrl">The <see cref="Url"/> of the current request, which usually contains the result of the OAuth workflow.</param>
		/// <returns>Returns the <see cref="Url"/> of the request before starting the OAuth workflow.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		protected override Result<Url> DoExchangeCodeForAccessToken(IMansionWebContext context, Url requestUrl)
		{
			// parse the query string
			var getParameters = requestUrl.QueryString;

			// extract the state and possible error codes
			var state = Url.CreateUrl(context);
			var errorReason = getParameters["error_reason"];
			var error = getParameters["error"];
			var errorDescription = getParameters["error_description"];

			// check for errors
			if (!string.IsNullOrEmpty(errorReason) || !string.IsNullOrEmpty(error) || !string.IsNullOrEmpty(errorDescription))
			{
				// authentication failed because user clicked cancel,
				// redirect back with reason
				var modifiableUrl = context.Request.RequestUrl.Clone();
				modifiableUrl.QueryString["status"] = "failed";
				modifiableUrl.QueryString["reason"] = "cancelled-by-user";
				return Result<Url>.Redirect(modifiableUrl);
			}

			// get the code and state
			var code = getParameters["code"];

			// creat the client
			var client = new RestClient(GraphApiEndpoint);

			// create the request
			var request = new RestRequest("oauth/access_token")
				.AddParameter("client_id", clientId)
				.AddParameter("redirect_Url", BuildExchangeTokenRedirectUrl(context).ToString())
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
			return Result<Url>.Success(state);
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
				var redirectUrl = BuildRedirectToFacebookDialogUrl(context, scope);

				// return the result
				return Result<TModel>.Redirect(redirectUrl);
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
					var redirectUrl = BuildRedirectToFacebookDialogUrl(context, scope);

					// return the result
					return Result<TModel>.Redirect(redirectUrl);
				}

				// unknown error
				throw new InvalidOperationException(string.Format("Facebook generated error {0} of type '{1}' with message '{2}'", response.Data.Error.Code, response.Data.Error.Type, response.Data.Error.Message));
			}

			// execute the request
			return Result<TModel>.Success(mapper(response.Data));
		}
		/// <summary>
		/// Builds the Facebook dialg <see cref="Url"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="scope"></param>
		/// <returns></returns>
		private Url BuildRedirectToFacebookDialogUrl(IMansionWebContext context, string scope = null)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// state
			var state = context.Request.RequestUrl;
			if (state == null)
				throw new InvalidOperationException("Must have a state");

			// construct the client
			var client = new RestClient(OAathDialogEndpoint);

			// create the request
			var request = new RestRequest()
				.AddParameter("client_id", clientId)
				.AddParameter("redirect_Url", BuildExchangeTokenRedirectUrl(context).ToString())
				.AddParameter("state", state.ToString())
				.AddParameter("display", "popup");

			// add the scope when there is one
			if (scope != null)
				request.AddParameter("scope", scope);

			// get the Url of the request
			return Url.ParseUri(context.Request.ApplicationUrl, client.BuildUri(request));
		}
		#endregion
		#region Private Fields
		private readonly string clientId;
		private readonly string clientSecret;
		#endregion
	}
}