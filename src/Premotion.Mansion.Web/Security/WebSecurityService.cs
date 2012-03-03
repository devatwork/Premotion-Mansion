using System;
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
		#region Initialize Methods
		/// <summary>
		/// Initializes the frontoffice user.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the initialized user.</returns>
		protected override UserState InitializeFrontofficeUser(MansionContext context)
		{
			return InitializeUserFromCookie(context, Constants.FrontofficeUserRevivalDataCookieName) ?? UserState.AnonymousUser;
		}
		/// <summary>
		/// Initializes the backoffice user.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the initialized user.</returns>
		protected override UserState InitializeBackofficeUser(MansionContext context)
		{
			return InitializeUserFromCookie(context, Constants.BackofficeUserRevivalDataCookieName) ?? UserState.AnonymousUser;
		}
		/// <summary>
		/// Tries to revive the user from cookie.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="cookieName">The name of the cookie from which to revive the user.</param>
		/// <returns>Returns the revived user or null.</returns>
		private UserState InitializeUserFromCookie(MansionContext context, string cookieName)
		{
			// get the web request context
			var httpContext = context.Cast<MansionWebContext>().HttpContext;
			if (!httpContext.HasSession)
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
			var conversionService = context.Nucleus.Get<IConversionService>(context);
			var revivalProperties = conversionService.Convert<IPropertyBag>(context, revivalCookie.Value, new PropertyBag());

			// get the authentication provider
			String authenticationProviderName;
			if (!revivalProperties.TryGetAndRemove(context, "authenticationProviderName", out authenticationProviderName) || string.IsNullOrEmpty(authenticationProviderName))
			{
				httpContext.DeleteCookie(cookieName);
				return null;
			}
			AuthenticationProvider authenticationProvider;
			if (TryResolveAuthenticationProvider(context, authenticationProviderName, out authenticationProvider))
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
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="authenicationProvider">The authentication provider which to use.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns the authenticated <see cref="UserState"/> or null.</returns>
		protected override UserState DoAuthenticate(MansionContext context, AuthenticationProvider authenicationProvider, IPropertyBag parameters)
		{
			// authenticate
			var user = authenicationProvider.Authenticate(context, parameters);
			if (user == null)
				return null;

			// get the web request context
			var httpContext = context.Cast<MansionWebContext>().HttpContext;

			// store this user in the session
			if (!httpContext.HasSession)
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
					var conversionService = context.Nucleus.Get<IConversionService>(context);
					var encryptedRevivalData = conversionService.Convert<string>(context, revivalData);

					// store it in a cookie
					var revivalCookie = new HttpCookie(revivalCookieName, encryptedRevivalData)
					                    {
					                    	Expires = DateTime.Now.AddDays(14)
					                    };
					httpContext.Response.SetCookie(revivalCookie);
				}
			}
			else
				httpContext.DeleteCookie(revivalCookieName);

			// authentication was successful
			return user;
		}
		/// <summary>
		/// Logs the user of from the current request context.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="authenicationProvider">The authentication provider which to use.</param>
		protected override void DoLogoff(MansionContext context, AuthenticationProvider authenicationProvider)
		{
			// authenticate
			authenicationProvider.Logoff(context);

			// get the web request context
			var httpContext = context.Cast<MansionWebContext>().HttpContext;

			// clear the user from the session
			if (httpContext.HasSession)
				httpContext.Session.Remove(GetRevivalCookieName(context));

			// delete any revival cookies
			httpContext.DeleteCookie(GetRevivalCookieName(context));
		}
		#endregion
		#region Revival Methods
		/// <summary>
		/// Gets the name of the revival cookie based on the context.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the name.</returns>
		private static string GetRevivalCookieName(MansionContext context)
		{
			return context.IsBackoffice ? Constants.BackofficeUserRevivalDataCookieName : Constants.FrontofficeUserRevivalDataCookieName;
		}
		#endregion
	}
}