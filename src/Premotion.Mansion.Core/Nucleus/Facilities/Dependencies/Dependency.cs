using System;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Dependencies
{
	/// <summary>
	/// Represents a dependency on another service.
	/// </summary>
	public class Dependency
	{
		#region Constructors
		/// <summary>
		/// Private constructor, use <see cref="Create{TServiceContract}"/>.
		/// </summary>
		private Dependency()
		{
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates an dependency on the specified service.
		/// </summary>
		/// <typeparam name="TServiceContract">The contract of the dependend service.</typeparam>
		/// <returns>Returns the created dependency.</returns>
		public static Dependency Create<TServiceContract>() where TServiceContract : IService
		{
			// create the dependency
			return new Dependency
			       {
			       	ContractType = typeof (TServiceContract)
			       };
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the contract of the dependend service.
		/// </summary>
		public Type ContractType { get; private set; }
		#endregion
	}
}