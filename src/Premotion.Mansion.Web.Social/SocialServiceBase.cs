using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Web.Urls;

namespace Premotion.Mansion.Web.Social
{
	/// <summary>
	/// Base class for all <see cref="ISocialService"/>s.
	/// </summary>
	public abstract class SocialServiceBase : ISocialService
	{
		#region Constructors
		/// <summary>
		/// Cosntructs this social service.
		/// </summary>
		/// <param name="providerName">The name of this <see cref="ISocialService"/>.</param>
		/// <param name="conversionService">The <see cref="IConversionService"/>.</param>
		protected SocialServiceBase(string providerName, IConversionService conversionService)
		{
			//  validate arguments
			if (string.IsNullOrEmpty(providerName))
				throw new ArgumentNullException("providerName");
			if (conversionService == null)
				throw new ArgumentNullException("conversionService");

			// set values
			ProviderName = providerName;
			this.conversionService = conversionService;
		}
		#endregion
		#region Implementation of ISocialService
		/// <summary>
		/// Retrieves the <see cref="Profile"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the <see cref="Result{Profile}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public Result<Profile> RetrieveProfile(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			try
			{
				return DoRetrieveProfile(context);
			}
			catch (Exception ex)
			{
				return Result<Profile>.Error(ex);
			}
		}
		/// <summary>
		/// Exchanges the OAuth code for an access token.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the <see cref="Url"/> of the request before starting the OAuth workflow.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public Result<Url> ExchangeCodeForAccessToken(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// retrieve the request Url
			var requestUrl = context.Request.Url;
			if (requestUrl == null)
				throw new InvalidOperationException("This is not a request");

			// invoke template method
			try
			{
				return DoExchangeCodeForAccessToken(context, requestUrl);
			}
			catch (Exception ex)
			{
				return Result<Url>.Error(ex);
			}
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Retrieves the <see cref="Profile"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the <see cref="Result{Profile}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		protected abstract Result<Profile> DoRetrieveProfile(IMansionWebContext context);
		/// <summary>
		/// Exchanges the OAuth code for an access token.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="requestUrl">The <see cref="Url"/> of the current request, which usually contains the result of the OAuth workflow.</param>
		/// <returns>Returns the <see cref="Url"/> of the request before starting the OAuth workflow.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		protected abstract Result<Url> DoExchangeCodeForAccessToken(IMansionWebContext context, Url requestUrl);
		#endregion
		#region Helper Methods
		/// <summary>
		/// Constructs a <see cref="Url"/> to which the OAuth provider will redirect.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the generated <see cref="Url"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		protected Url BuildExchangeTokenRedirectUrl(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// build the redirect Url
			return RouteUrlBuilder.BuildRoute(context, "OAuth", "ExchangeCodeForAccessToken", ProviderName);
		}
		/// <summary>
		/// Sets an OAuth variable,
		/// </summary>
		/// <typeparam name="TValue">The type of variable.</typeparam>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="name">The name of the variable.</param>
		/// <param name="value">The <typeparamref name="TValue"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="name"/> or <paramref name="value"/> is null.</exception>
		protected void SetOAuthVariable<TValue>(IMansionWebContext context, string name, TValue value) where TValue : class
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (value == null)
				throw new ArgumentNullException("value");

			// get the session
			var session = context.Session;

			// set the value
			session[ProviderName + "_" + name] = value;
		}
		/// <summary>
		/// Try to get a value name.
		/// </summary>
		/// <typeparam name="TValue">The type of value.</typeparam>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="name">The name of the variable.</param>
		/// <param name="value">The value when found.</param>
		/// <returns>Returns true when a value was found, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="name"/> or <paramref name="value"/> is null.</exception>
		protected bool TryGetOAuthVariable<TValue>(IMansionWebContext context, string name, out TValue value) where TValue : class
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// get the session
			var session = context.Session;

			// get the value
			var valueObject = session[ProviderName + "_" + name];
			if (valueObject == null)
			{
				value = null;
				return false;
			}

			// return the result
			value = conversionService.Convert<TValue>(context, valueObject);
			return true;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this <see cref="ISocialService"/>.
		/// </summary>
		public string ProviderName { get; private set; }
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}