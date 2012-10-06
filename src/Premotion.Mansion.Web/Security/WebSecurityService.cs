using System;
using System.Collections.Generic;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Web.Security
{
	/// <summary>
	/// Implements <see cref="ISecurityService"/> for web requests.
	/// </summary>
	public class WebSecurityService : SecurityServiceBase
	{
		#region Constructors
		/// <summary>
		/// Constructs the web security service.
		/// </summary>
		/// <param name="conversionService">The <see cref="IConversionService"/>.</param>
		/// <param name="authenticationProviders">The <see cref="IEnumerable{T}"/>s.</param>
		public WebSecurityService(IConversionService conversionService, IEnumerable<AuthenticationProvider> authenticationProviders) : base(authenticationProviders)
		{
			//validate arguments
			if (conversionService == null)
				throw new ArgumentNullException("conversionService");

			// set values
			this.conversionService = conversionService;
		}
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Initializes the frontoffice user.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the initialized user.</returns>
		protected override UserState InitializeFrontofficeUser(IMansionContext context)
		{
			return InitializeUserFromCookie(context, Constants.FrontofficeUserRevivalDataCookieName) ?? UserState.AnonymousUser;
		}
		/// <summary>
		/// Initializes the backoffice user.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the initialized user.</returns>
		protected override UserState InitializeBackofficeUser(IMansionContext context)
		{
			return InitializeUserFromCookie(context, Constants.BackofficeUserRevivalDataCookieName) ?? UserState.AnonymousUser;
		}
		/// <summary>
		/// Tries to revive the user from cookie.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="cookieName">The name of the cookie from which to revive the user.</param>
		/// <returns>Returns the revived user or null.</returns>
		private UserState InitializeUserFromCookie(IMansionContext context, string cookieName)
		{
			// get the web request context
			var httpContext = context.Cast<IMansionWebContext>().HttpContext;
			if (!httpContext.HasSession())
				return null;

			// check sesssion
			var sessionUser = httpContext.Session[cookieName] as UserState;
			if (sessionUser != null)
				return sessionUser;

			// check for revival cookie
			var revivalCookie = httpContext.Request.Cookies[cookieName];
			if (revivalCookie == null || string.IsNullOrEmpty(revivalCookie.Value))
				return null;

			// deserialize the properties, TODO: add proper decryption, TODO: check for cookie theft
			var revivalProperties = conversionService.Convert<IPropertyBag>(context, revivalCookie.Value, new PropertyBag());

			// get the authentication provider
			String authenticationProviderName;
			if (!revivalProperties.TryGetAndRemove(context, "authenticationProviderName", out authenticationProviderName) || string.IsNullOrEmpty(authenticationProviderName))
			{
				httpContext.DeleteCookie(cookieName);
				return null;
			}
			AuthenticationProvider authenticationProvider;
			if (!TryResolveAuthenticationProvider(context, authenticationProviderName, out authenticationProvider))
			{
				httpContext.DeleteCookie(cookieName);
				return null;
			}

			// try to revive the user
			var revivedUser = authenticationProvider.ReviveUser(context, revivalProperties);
			if (revivedUser == null)
				httpContext.DeleteCookie(cookieName);
			return revivedUser;
		}
		#endregion
		#region User Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="authenicationProvider">The authentication provider which to use.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns the <see cref="AuthenticationResult"/>.</returns>
		protected override AuthenticationResult DoAuthenticate(IMansionContext context, AuthenticationProvider authenicationProvider, IPropertyBag parameters)
		{
			// authenticate
			var result = authenicationProvider.Authenticate(context, parameters);
			if (!result.WasSuccesful)
				return result;
			var user = result.UserState;

			// get the web request context
			var httpContext = context.Cast<IMansionWebContext>().HttpContext;

			// store this user in the session
			if (!httpContext.HasSession())
				return null;
			httpContext.Session.Add(GetRevivalCookieName(context), user);

			// check if the authentication provider support user revival and the rememberMe flag was set
			var revivalCookieName = GetRevivalCookieName(context);
			if (authenicationProvider.SupportsRevival && parameters.Get(context, "allowRevival", false))
			{
				// get the revival data for this user
				var revivalData = authenicationProvider.GetRevivalProperties(context, user, parameters);
				if (revivalData != null)
				{
					// add additional revival properties
					revivalData.Set("authenticationProviderName", authenicationProvider.Name);

					// encrypt it, TODO: add proper encryption
					var encryptedRevivalData = conversionService.Convert<string>(context, revivalData);

					// store it in a cookie
					var revivalCookie = new HttpCookie(revivalCookieName, encryptedRevivalData)
					                    {
					                    	Expires = DateTime.Now.AddDays(14),
												HttpOnly = true
					                    };
					httpContext.Response.SetCookie(revivalCookie);
				}
			}
			else
				httpContext.DeleteCookie(revivalCookieName);

			// authentication was successful
			return result;
		}
		/// <summary>
		/// Logs the user of from the current request context.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="authenicationProvider">The authentication provider which to use.</param>
		protected override void DoLogoff(IMansionContext context, AuthenticationProvider authenicationProvider)
		{
			// authenticate
			authenicationProvider.Logoff(context);

			// get the web request context
			var httpContext = context.Cast<IMansionWebContext>().HttpContext;

			// clear the user from the session
			if (httpContext.HasSession())
				httpContext.Session.Remove(GetRevivalCookieName(context));

			// delete any revival cookies
			httpContext.DeleteCookie(GetRevivalCookieName(context));
		}
		#endregion
		#region Revival Methods
		/// <summary>
		/// Gets the name of the revival cookie based on the context.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the name.</returns>
		private static string GetRevivalCookieName(IMansionContext context)
		{
			return context.IsBackoffice ? Constants.BackofficeUserRevivalDataCookieName : Constants.FrontofficeUserRevivalDataCookieName;
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}