using System.Collections.Generic;
using System.Reflection;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Provides reflections services.
	/// </summary>
	public interface IReflectionService
	{
		#region Initialized Methods
		/// <summary>
		/// Initializes the reflection service, which registers all the exported types with the given <paramref name="assemblies"/>.
		/// </summary>
		/// <param name="nucleus"> </param>
		/// <param name="assemblies"></param>
		void Initialize(IConfigurableNucleus nucleus, IEnumerable<Assembly> assemblies);
		#endregion
		#region Properties
		/// <summary>
		/// Gets all the assemblies loaded by this application in a bottom-to-top order. The top assembly is the most specific assembly.
		/// </summary>
		IEnumerable<Assembly> Assemblies { get; }
		/// <summary>
		/// Gets all the assemblies loaded by this application in a top-to-bottom order. The top assembly is the most specific assembly.
		/// </summary>
		IEnumerable<Assembly> AssembliesReversed { get; }
		#endregion
	}
}