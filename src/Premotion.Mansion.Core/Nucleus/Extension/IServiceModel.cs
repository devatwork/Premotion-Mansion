using System;

namespace Premotion.Mansion.Core.Nucleus.Extension
{
	/// <summary>
	/// Represents a description of a particular service.
	/// </summary>
	public interface IServiceModel
	{
		#region Service Methods
		/// <summary>
		/// Gets the instance of the service.
		/// </summary>
		/// <typeparam name="TContract">The type of <see cref="IService"/> which to get.</typeparam>
		/// <returns>Returns the service of this model.</returns>
		/// <exception cref="ArgumentException">Thrown when the service instance does not implement the service contract.</exception>
		TContract GetInstance<TContract>() where TContract : IService;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the contract <see cref="Type"/> of the service this model describes.
		/// </summary>
		Type ContractType { get; }
		/// <summary>
		/// Gets the underlying implementation <see cref="Type"/> of the service.
		/// </summary>
		Type ImplementationType { get; }
		#endregion
	}
}