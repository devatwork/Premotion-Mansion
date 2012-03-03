using System;

namespace Premotion.Mansion.Core.Attributes
{
	/// <summary>
	/// Marks any type as exported indicating.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class ExportedAttribute : Attribute
	{
	}
}