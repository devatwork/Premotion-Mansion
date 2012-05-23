using System;

namespace Premotion.Mansion.Web.Social
{
	/// <summary>
	/// Provides commom methods for social services.
	/// </summary>
	public interface ISocialService
	{
		#region Social Methods
		/// <summary>
		/// Retrieves the <see cref="Profile"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the <see cref="Result{Profile}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		Result<Profile> RetrieveProfile(IMansionWebContext context);
		/// <summary>
		/// Exchanges the OAuth code for an access token.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the <see cref="Uri"/> of the request before starting the OAuth workflow.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		Result<Uri> ExchangeCodeForAccessToken(IMansionWebContext context);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this <see cref="ISocialService"/>.
		/// </summary>
		string ProviderName { get; }
		#endregion
	}
}