using System;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Defines an authentication provider.
	/// </summary>
	[Exported(typeof (AuthenticationProvider))]
	public abstract class AuthenticationProvider
	{
		#region Constructors
		/// <summary>
		/// Constructs an authentication provider.
		/// </summary>
		/// <param name="authenticationProviderName">The name of the provider.</param>
		protected AuthenticationProvider(string authenticationProviderName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(authenticationProviderName))
				throw new ArgumentNullException("authenticationProviderName");

			// set values
			name = authenticationProviderName;
		}
		#endregion
		#region Authentication Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns the user when authenticated otherwise null.</returns>
		public UserState Authenticate(IMansionContext context, IPropertyBag parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			// invoke template method
			return DoAuthenticate(context, parameters);
		}
		/// <summary>
		/// Logs the current user off.
		/// </summary>
		/// <param name="context">The security context.</param>
		public void Logoff(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			DoLogoff(context);
		}
		/// <summary>
		/// Template method for <see cref="AuthenticationProvider.Authenticate"/>.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns the user when authenticated otherwise null.</returns>
		protected abstract UserState DoAuthenticate(IMansionContext context, IPropertyBag parameters);
		/// <summary>
		/// Template method for <see cref="AuthenticationProvider.Logoff"/>.
		/// </summary>
		/// <param name="context">The security context.</param>
		protected abstract void DoLogoff(IMansionContext context);
		#endregion
		#region Revival Methods
		/// <summary>
		/// Gets the data used to revive users.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <param name="userState">The user which will be revived.</param>
		/// <param name="parameters">The parameters used for authentication of <paramref name="userState"/>.</param>
		/// <returns>Returns the revival data or null.</returns>
		/// <exception cref="NotSupportedException">Thrown when <see cref="SupportsRevival"/> is false.</exception>
		public virtual IPropertyBag GetRevivalProperties(IMansionContext context, UserState userState, IPropertyBag parameters)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// Tries to revive the user based on the revival properties.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <param name="revivalProperties">The revival properties.</param>
		/// <returns>Returns the revived <see cref="UserState"/> or null.</returns>
		public virtual UserState ReviveUser(IMansionContext context, IPropertyBag revivalProperties)
		{
			throw new NotSupportedException();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether this authentication provider supports user revival.
		/// </summary>
		public virtual bool SupportsRevival
		{
			get { return false; }
		}
		/// <summary>
		/// Gets the name of this authentication provider.
		/// </summary>
		public string Name
		{
			get { return name; }
		}
		#endregion
		#region Private Fields
		private readonly string name;
		#endregion
	}
}