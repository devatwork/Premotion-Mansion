using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Implements the base class of all type descriptors.
	/// </summary>
	public abstract class TypeDescriptor : Descriptor
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="ITypeDefinition"/> to which this descriptor is applied.
		/// </summary>
		public ITypeDefinition TypeDefinition { get; set; }
		#endregion
	}
}