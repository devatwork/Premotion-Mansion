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
		/// <returns>Returns the user when authenticated otherwise null.</returns>
		protected override UserState DoAuthenticate(MansionContext context, IPropertyBag parameters)
		{
			// get the credentials
			var username = parameters.Get<string>(context, "username");
			if (string.IsNullOrEmpty(username))
				return null;
			var password = parameters.Get<string>(context, "password");
			if (string.IsNullOrEmpty(password))
				return null;

			// check for incorrect credentials
			if (!username.Equals("System") || !password.Equals("Premotion"))
				return null;

			// return the user
			return new UserState(username, this);
		}
		/// <summary>
		/// Logs the current user off.
		/// </summary>
		/// <param name="context">The security context.</param>
		protected override void DoLogoff(MansionContext context)
		{
			// do nothing special
		}
		#endregion
	}
}