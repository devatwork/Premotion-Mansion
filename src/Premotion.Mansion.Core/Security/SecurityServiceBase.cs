using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Implements a base class for classes implementing <see cref="ISecurityService"/>.
	/// </summary>
	public abstract class SecurityServiceBase : ManagedLifecycleService, ISecurityService, IServiceWithDependencies
	{
		#region Initialize Methods
		/// <summary>
		/// Initializes the security for the specified <see cref="MansionContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		public void InitializeSecurityContext(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

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
		protected abstract UserState InitializeFrontofficeUser(MansionContext context);
		/// <summary>
		/// Initializes the backoffice user.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <returns>Returns the initialized user.</returns>
		protected abstract UserState InitializeBackofficeUser(MansionContext context);
		#endregion
		#region User Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="authenticationProviderName">The name of the authentication provider.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns true when the authentication was succeful, otherwise false.</returns>
		public bool Authenticate(MansionContext context, string authenticationProviderName, IPropertyBag parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(authenticationProviderName))
				throw new ArgumentNullException("authenticationProviderName");
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			CheckDisposed();

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
		/// Logs the user of from the current <see cref="MansionContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		public void Logoff(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

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
		protected abstract UserState DoAuthenticate(MansionContext securityContext, AuthenticationProvider authenicationProvider, IPropertyBag parameters);
		/// <summary>
		/// Logs the user of from the current <see cref="IContext"/>.
		/// </summary>
		/// <param name="securityContext">The security context.</param>
		/// <param name="authenicationProvider">The authentication provider which to use.</param>
		protected abstract void DoLogoff(MansionContext securityContext, AuthenticationProvider authenicationProvider);
		#endregion
		#region Resolve Methods
		/// <summary>
		/// Resolves <paramref name="authenticationProviderName"/> to an actual implementation of <see cref="AuthenticationProvider" />.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="authenticationProviderName">The name of the desired authentication provider.</param>
		/// <returns>Returns the <see cref="AuthenticationProvider"/>.</returns>
		protected AuthenticationProvider ResolveAuthenticationProvider(MansionContext context, string authenticationProviderName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(authenticationProviderName))
				throw new ArgumentNullException("authenticationProviderName");
			CheckDisposed();

			AuthenticationProvider provider;
			if (!TryResolveAuthenticationProvider(context, authenticationProviderName, out provider))
				throw new InvalidOperationException(string.Format("Could not find authentication provider with name '{0}'", authenticationProviderName));
			return provider;
		}
		/// <summary>
		/// Resolves <paramref name="authenticationProviderName"/> to an actual implementation of <see cref="AuthenticationProvider" />.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="authenticationProviderName">The name of the desired authentication provider.</param>
		/// <param name="provider">The <see cref="AuthenticationProvider"/> found.</param>
		/// <returns>Returns true when the provider was found, otherwise false.</returns>
		protected bool TryResolveAuthenticationProvider(MansionContext context, string authenticationProviderName, out AuthenticationProvider provider)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(authenticationProviderName))
				throw new ArgumentNullException("authenticationProviderName");
			CheckDisposed();

			return providers.TryGetValue(authenticationProviderName, out provider);
		}
		#endregion
		#region Implementation of IStartableService
		/// <summary>
		/// Invoked just before this service is used for the first time.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		protected override void DoStart(INucleusAwareContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the naming and object factory services
			var namingService = context.Nucleus.Get<ITypeDirectoryService>(context);
			var objectFactoryService = context.Nucleus.Get<IObjectFactoryService>(context);

			// look up all the types implementing 
			foreach (var provider in  objectFactoryService.Create<AuthenticationProvider>(namingService.Lookup<AuthenticationProvider>()))
				providers.Add(provider.Name, provider);
		}
		#endregion
		#region Implementation of IServiceWithDependencies
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		public DependencyModel Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Private Fields
		private static readonly DependencyModel dependencies = new DependencyModel().Add<ITypeDirectoryService>().Add<IObjectFactoryService>();
		private readonly Dictionary<string, AuthenticationProvider> providers = new Dictionary<string, AuthenticationProvider>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}