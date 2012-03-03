using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Represents a property of a type.
	/// </summary>
	public interface IPropertyDefinition : IDescriptee
	{
		#region Properties
		/// <summary>
		/// Gets the name of this property.
		/// </summary>
		string Name { get; }
		#endregion
	}
}