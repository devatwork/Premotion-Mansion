using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Decorated classes are identified as type descriptors.
	/// </summary>
	public class TypeDescriptorAttribute : DescriptorAttribute
	{
		#region Constructors
		/// <summary>
		/// Constructs a type descriptor attribute.
		/// </summary>
		/// <param name="namespaceUri">The namespace in which the descriptor.</param>
		/// <param name="name">The name of the descriptor.</param>
		public TypeDescriptorAttribute(string namespaceUri, string name) : base(typeof (TypeDescriptor), namespaceUri, name)
		{
		}
		#endregion
	}
}