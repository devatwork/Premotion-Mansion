using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Manages services and extensions.
	/// </summary>
	public interface INucleus
	{
		#region Service Methods
		/// <summary>
		/// Gets a <see cref="IService"/> of type TService.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="contract">The contract of the service, must inherit from <see cref="IService"/>.</param>
		/// <returns>Returns the instance of the service.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="contract"/> does not inherit from <see cref="IService"/>.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is null.</exception>
		/// <exception cref="AmbiguousServiceFoundException">Thrown when TService did not resolve to a service instance.</exception>
		/// <exception cref="InvalidServiceStateException">Thrown when TService resolved to mulitple service instances.</exception>
		/// <exception cref="NoServiceFoundException">Thrown when TService resolved to a service which is not ready for use.</exception>
		IService Get(IContext context, Type contract);
		/// <summary>
		/// Gets a <see cref="IService"/> of type TService.
		/// </summary>
		/// <typeparam name="TContract">The type of <see cref="IService"/> which to get.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns the instance of the service.</returns>
		/// <exception cref="AmbiguousServiceFoundException">Thrown when TService did not resolve to a service instance.</exception>
		/// <exception cref="InvalidServiceStateException">Thrown when TService resolved to mulitple service instances.</exception>
		/// <exception cref="NoServiceFoundException">Thrown when TService resolved to a service which is not ready for use.</exception>
		TContract Get<TContract>(IContext context) where TContract : IService;
		#endregion
	}
}