using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Web.Social
{
	/// <summary>
	/// Implements <see cref="ISocialServiceDiscoveryService"/>.
	/// </summary>
	public class SocialServiceDiscoveryService : ISocialServiceDiscoveryService
	{
		#region Constructors
		/// <summary>
		/// Constructs the social service discovery service.
		/// </summary>
		/// <param name="services">The social services.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is null.</exception>
		public SocialServiceDiscoveryService(IEnumerable<ISocialService> services)
		{
			// validate arguments
			if (services == null)
				throw new ArgumentNullException("services");

			// turn into lookup table
			this.services = services.ToDictionary(entry => entry.ProviderName, StringComparer.OrdinalIgnoreCase);
		}
		#endregion
		#region Implementation of ISocialServiceDiscoveryService
		/// <summary>
		/// Locates the <see cref="ISocialService"/> by it's <paramref name="serviceName"/>.
		/// </summary>
		/// <param name="serviceName">The name of the social service.</param>
		/// <returns>Returns the instance of the <see cref="ISocialService"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceName"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown when a <see cref="ISocialService"/> with the <paramref name="serviceName"/> is not found.</exception>
		public ISocialService Locate(string serviceName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(serviceName))
				throw new ArgumentNullException("serviceName");

			// get the service
			ISocialService service;
			if (!services.TryGetValue(serviceName, out service))
				throw new InvalidOperationException(string.Format("Could not find social service with name {0}", serviceName));

			return service;
		}
		#endregion
		#region Private Fields
		private readonly IDictionary<string, ISocialService> services;
		#endregion
	}
}