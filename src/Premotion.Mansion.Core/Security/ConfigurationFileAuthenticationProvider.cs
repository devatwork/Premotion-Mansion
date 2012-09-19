using System;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Implements <see cref="AuthenticationProvider"/> using credentials specified in the configuration file of this application.
	/// </summary>
	public class ConfigurationFileAuthenticationProvider : AuthenticationProvider
	{
		#region Constructors
		/// <summary>
		/// Constructs an authentication provider.
		/// </summary>
		public ConfigurationFileAuthenticationProvider() : base("ConfigurationFileAuthenticationProvider")
		{
		}
		#endregion
		#region Authentication Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns the <see cref="AuthenticationResult"/>.</returns>
		protected override AuthenticationResult DoAuthenticate(IMansionContext context, IPropertyBag parameters)
		{
			// get the credentials
			var username = parameters.Get<string>(context, "username");
			if (string.IsNullOrEmpty(username))
			{
				return AuthenticationResult.Failed(new PropertyBag
				                                   {
				                                   	{AuthenticationResult.ReasonPropertyName, AuthenticationResult.NoUsernameSpecifiedReason}
				                                   });
			}
			var password = parameters.Get<string>(context, "password");
			if (string.IsNullOrEmpty(password))
			{
				return AuthenticationResult.Failed(new PropertyBag
				                                   {
				                                   	{AuthenticationResult.ReasonPropertyName, AuthenticationResult.NoPasswordSpecifiedReason}
				                                   });
			}

			// check for incorrect credentials
			if (!username.Equals("System") || !password.Equals("Premotion"))
			{
				return AuthenticationResult.Failed(new PropertyBag
				                                   {
				                                   	{AuthenticationResult.ReasonPropertyName, AuthenticationResult.InvalidCredentialsReason}
				                                   });
			}

			// return the user
			return AuthenticationResult.Success(new UserState(Guid.NewGuid(), this), new PropertyBag());
		}
		/// <summary>
		/// Logs the current user off.
		/// </summary>
		/// <param name="context">The security context.</param>
		protected override void DoLogoff(IMansionContext context)
		{
			// do nothing special
		}
		#endregion
	}
}