using System;
using System.Reflection;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Reflection
{
	/// <summary>
	/// Represents a registered assembly.
	/// </summary>
	public class AssemblyModel
	{
		#region Constructors
		/// <summary>
		/// Constructs an assembly model.
		/// </summary>
		/// <param name="name">The name of the assembly.</param>
		/// <param name="assembly">The actual <see cref="Assembly"/>.</param>
		private AssemblyModel(string name, Assembly assembly)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			// set values
			Name = name;
			Assembly = assembly;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates a model representation of an assembly.
		/// </summary>
		/// <typeparam name="TType">This parameter is used to discover the <see cref="Assembly"/> which to register.</typeparam>
		/// <param name="name">The name of this model.</param>
		/// <returns>Returns the created model.</returns>
		public static AssemblyModel Create<TType>(string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			return new AssemblyModel(name, typeof (TType).Assembly);
		}
		/// <summary>
		/// Creates a model representation of an assembly.
		/// </summary>
		/// <param name="name">The name of this model.</param>
		/// <param name="assemblyFile">The name of the assembly which to load.</param>
		/// <returns>Returns the created model.</returns>
		public static AssemblyModel Create(string name, string assemblyFile)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(assemblyFile))
				throw new ArgumentNullException("assemblyFile");

			return new AssemblyModel(name, Assembly.Load(assemblyFile));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the path to this assembly.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets <see cref="Assembly"/> this model describes.
		/// </summary>
		public Assembly Assembly { get; private set; }
		#endregion
	}
}