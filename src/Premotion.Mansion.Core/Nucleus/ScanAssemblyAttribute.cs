using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Marks an assembly as exported.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class ScanAssemblyAttribute : Attribute
	{
		#region Constructors
		/// <summary>
		/// Constructs an scan assembly attribute.
		/// </summary>
		/// <param name="priority">This priority of this assembly. Higher prioritised assemblies override exported types of lower prioritised assemblies.</param>
		public ScanAssemblyAttribute(int priority)
		{
			// set values
			Priority = priority;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the priority of this assembly. Higher prioritised assemblies override exported types of lower prioritised assemblies.
		/// </summary>
		public int Priority { get; private set; }
		#endregion
	}
}