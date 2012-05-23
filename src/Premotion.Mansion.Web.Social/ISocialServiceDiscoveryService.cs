using System;

namespace Premotion.Mansion.Web.Social
{
	/// <summary>
	/// Provides discovery facilities for <see cref="ISocialService"/>.
	/// </summary>
	public interface ISocialServiceDiscoveryService
	{
		#region Resolve Methods
		/// <summary>
		/// Locates the <see cref="ISocialService"/> by it's <paramref name="serviceName"/>.
		/// </summary>
		/// <param name="serviceName">The name of the social service.</param>
		/// <returns>Returns the instance of the <see cref="ISocialService"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceName"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown when a <see cref="ISocialService"/> with the <paramref name="serviceName"/> is not found.</exception>
		ISocialService Locate(string serviceName);
		#endregion
	}
}