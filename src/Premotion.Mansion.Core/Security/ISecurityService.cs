namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Defines the security service, which can authenticate users.
	/// </summary>
	public interface ISecurityService
	{
		#region Initialize Methods
		/// <summary>
		/// Initializes the security for the specified <see cref="IMansionContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		void InitializeSecurityContext(IMansionContext context);
		#endregion
		#region User Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="authenticationProviderName">The name of the authentication provider.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns the <see cref="AuthenticationResult"/>.</returns>
		AuthenticationResult Authenticate(IMansionContext context, string authenticationProviderName, IPropertyBag parameters);
		/// <summary>
		/// Logs the user of from the current <see cref="IMansionContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		void Logoff(IMansionContext context);
		#endregion
	}
}