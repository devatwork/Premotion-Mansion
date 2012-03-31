using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Implements a base class for classes implementing <see cref="ISecurityService"/>.
	/// </summary>
	public abstract class SecurityServiceBase : ISecurityService
	{
		#region Constructors
		/// <summary>
		/// Constructs the security base service.
		/// </summary>
		/// <param name="authenticationProviders">The <see cref="IEnumerable{T}"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="authenticationProviders"/> is null.</exception>
		protected SecurityServiceBase(IEnumerable<AuthenticationProvider> authenticationProviders)
		{
			// validate arguments
			if (authenticationProviders == null)
				throw new ArgumentNullException("authenticationProviders");

			// set values
			providers = authenticationProviders.ToDictionary(provider => provider.Name);
		}
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Initializes the security for the specified <see cref="IMansionContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void InitializeSecurityContext(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// set users
			context.SetFrontofficeUserState(InitializeFrontofficeUser(context));
			context.SetBackofficeUserState(InitializeBackofficeUser(context));

			// push the users to the stack
			var frontofficeUserProperties = new PropertyBag(context.FrontofficeUserState.Properties)
			                                {
			                                	{"id", context.FrontofficeUserState.Id},
			                                	{"isAuthenticated", context.FrontofficeUserState.IsAuthenticated}
			                                };
			var backofficeUserProperties = new PropertyBag(context.BackofficeUserState.Properties)
			                               {
			                               	{"id", context.BackofficeUserState.Id},
			                               	{"isAuthenticated", context.BackofficeUserState.IsAuthenticated}
			                               };
			context.Stack.Push("FrontofficeUser", frontofficeUserProperties, true).Dispose();
			context.Stack.Push("BackofficeUser", backofficeUserProperties, true).Dispose();
			context.Stack.Push("User", context.IsBackoffice ? backofficeUserProperties : frontofficeUserProperties, true).Dispose();
		}
		/// <summary>
		/// Initializes the frontoffice user.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <returns>Returns the initialized user.</returns>
		protected abstract UserState InitializeFrontofficeUser(IMansionContext context);
		/// <summary>
		/// Initializes the backoffice user.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <returns>Returns the initialized user.</returns>
		protected abstract UserState InitializeBackofficeUser(IMansionContext context);
		#endregion
		#region User Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="authenticationProviderName">The name of the authentication provider.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns true when the authentication was succeful, otherwise false.</returns>
		public bool Authenticate(IMansionContext context, string authenticationProviderName, IPropertyBag parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(authenticationProviderName))
				throw new ArgumentNullException("authenticationProviderName");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			// get the authentication provider
			var authenicationProvider = ResolveAuthenticationProvider(context, authenticationProviderName);

			// invoke template method
			var authenticatedUser = DoAuthenticate(context, authenicationProvider, parameters);
			if (authenticatedUser == null)
				return false;

			// set the user on the context
			context.SetCurrentUserState(authenticatedUser);

			// authentication successful
			return true;
		}
		/// <summary>
		/// Logs the user of from the current <see cref="IMansionContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void Logoff(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the authentication provider name of the user being logged off
			var authenticationProviderName = context.CurrentUserState.AuthenticationProviderName;
			var authenicationProvider = ResolveAuthenticationProvider(context, authenticationProviderName);

			// invoke template method
			DoLogoff(context, authenicationProvider);

			// set the anonymous user
			context.SetCurrentUserState(UserState.AnonymousUser);
		}
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="securityContext">The security context.</param>
		/// <param name="authenicationProvider">The authentication provider which to use.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns the authenticated <see cref="UserState"/> or null.</returns>
		protected abstract UserState DoAuthenticate(IMansionContext securityContext, AuthenticationProvider authenicationProvider, IPropertyBag parameters);
		/// <summary>
		/// Logs the user of from the current <see cref="IMansionContext"/>.
		/// </summary>
		/// <param name="securityContext">The security context.</param>
		/// <param name="authenicationProvider">The authentication provider which to use.</param>
		protected abstract void DoLogoff(IMansionContext securityContext, AuthenticationProvider authenicationProvider);
		#endregion
		#region Resolve Methods
		/// <summary>
		/// Resolves <paramref name="authenticationProviderName"/> to an actual implementation of <see cref="AuthenticationProvider" />.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="authenticationProviderName">The name of the desired authentication provider.</param>
		/// <returns>Returns the <see cref="AuthenticationProvider"/>.</returns>
		protected AuthenticationProvider ResolveAuthenticationProvider(IMansionContext context, string authenticationProviderName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(authenticationProviderName))
				throw new ArgumentNullException("authenticationProviderName");

			AuthenticationProvider provider;
			if (!TryResolveAuthenticationProvider(context, authenticationProviderName, out provider))
				throw new InvalidOperationException(string.Format("Could not find authentication provider with name '{0}'", authenticationProviderName));
			return provider;
		}
		/// <summary>
		/// Resolves <paramref name="authenticationProviderName"/> to an actual implementation of <see cref="AuthenticationProvider" />.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="authenticationProviderName">The name of the desired authentication provider.</param>
		/// <param name="provider">The <see cref="AuthenticationProvider"/> found.</param>
		/// <returns>Returns true when the provider was found, otherwise false.</returns>
		protected bool TryResolveAuthenticationProvider(IMansionContext context, string authenticationProviderName, out AuthenticationProvider provider)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(authenticationProviderName))
				throw new ArgumentNullException("authenticationProviderName");

			return providers.TryGetValue(authenticationProviderName, out provider);
		}
		#endregion
		#region Private Fields
		private readonly Dictionary<string, AuthenticationProvider> providers;
		#endregion
	}
}