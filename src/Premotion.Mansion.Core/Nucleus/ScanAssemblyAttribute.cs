using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Marks an assembly as exported.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class ScanAssemblyAttribute : Attribute
	{
	}
}