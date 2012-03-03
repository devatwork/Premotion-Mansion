using System.Collections.Generic;
using System.Reflection;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Reflection
{
	/// <summary>
	/// Defines the service for interacting with assemblies within a <see cref="INucleus"/>.
	/// </summary>
	public interface IAssemblyRegistrationService : IService
	{
		#region Register Methods
		/// <summary>
		/// Registers a <paramref name="model"/> of an <see cref="Assembly"/> in this service.
		/// </summary>
		/// <param name="model">The <see cref="AssemblyModel"/> which to register.</param>
		void RegisterAssembly(AssemblyModel model);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="AssemblyModel"/>s registered by this service.
		/// </summary>
		IEnumerable<AssemblyModel> RegisteredAssemblies { get; }
		#endregion
	}
}