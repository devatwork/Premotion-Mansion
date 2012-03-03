using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Defines the security service, which can authenticate users.
	/// </summary>
	public interface ISecurityService : IService
	{
		#region Initialize Methods
		/// <summary>
		/// Initializes the security for the specified <see cref="MansionContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		void InitializeSecurityContext(MansionContext context);
		#endregion
		#region User Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="authenticationProviderName">The name of the authentication provider.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns true when the authentication was succeful, otherwise false.</returns>
		bool Authenticate(MansionContext context, string authenticationProviderName, IPropertyBag parameters);
		/// <summary>
		/// Logs the user of from the current <see cref="MansionContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		void Logoff(MansionContext context);
		#endregion
	}
}