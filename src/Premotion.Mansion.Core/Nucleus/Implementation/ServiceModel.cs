using System;
using Premotion.Mansion.Core.Nucleus.Extension;

namespace Premotion.Mansion.Core.Nucleus.Implementation
{
	/// <summary>
	/// Represents a description of a particular service.
	/// </summary>
	public class ServiceModel : IServiceModel
	{
		#region Constructors
		/// <summary>
		/// Constructs a service model.
		/// </summary>
		/// <param name="contractType">The contract <see cref="Type"/>.</param>
		/// <param name="instance">The actuel implementation of the <see cref="IService"/>.</param>
		public ServiceModel(Type contractType, IService instance)
		{
			// validate arguments
			if (contractType == null)
				throw new ArgumentNullException("contractType");
			if (instance == null)
				throw new ArgumentNullException("instance");

			// make sure the instance actually implements the contract
			if (!contractType.IsAssignableFrom(instance.GetType()))
				throw new InvalidOperationException(string.Format("Service implementation '{0}' does not implement contract '{1}'.", ImplementationType, contractType));

			// set values
			ContractType = contractType;
			ImplementationType = instance.GetType();
			this.instance = instance;
		}
		#endregion
		#region Service Methods
		/// <summary>
		/// Gets the instance of the service.
		/// </summary>
		/// <typeparam name="TContract">The type of <see cref="IService"/> which to get.</typeparam>
		/// <returns>Returns the service of this model.</returns>
		/// <exception cref="ArgumentException">Thrown when the service instance does not implement the service contract.</exception>
		public TContract GetInstance<TContract>() where TContract : IService
		{
			// check type
			if (!typeof (TContract).IsAssignableFrom(ImplementationType))
				throw new ArgumentException(string.Format("Can not get contract instance '{0}' from implementation '{1}'.", typeof (TContract), ImplementationType));

			// return the typed service
			return (TContract) instance;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Get/sets a flag indicating whether this service is started or not.
		/// </summary>
		public bool Activated { get; set; }
		/// <summary>
		/// Gets the contract <see cref="Type"/> of the service this model describes.
		/// </summary>
		public Type ContractType { get; private set; }
		/// <summary>
		/// Gets the underlying implementation <see cref="Type"/> of the service.
		/// </summary>
		public Type ImplementationType { get; private set; }
		#endregion
		#region Private Fields
		private readonly IService instance;
		#endregion
	}
}