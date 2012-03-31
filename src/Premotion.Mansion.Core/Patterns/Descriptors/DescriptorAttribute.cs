using System;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Patterns.Descriptors
{
	/// <summary>
	/// Decorated classes are identified as descriptors.
	/// </summary>
	public class DescriptorAttribute : NamedAttribute
	{
		#region Constructors
		/// <summary>
		/// Constructs a type descriptor attribute.
		/// </summary>
		/// <param name="namespaceUri">The namespace in which the descriptor.</param>
		/// <param name="name">The name of the descriptor.</param>
		public DescriptorAttribute(string namespaceUri, string name) : this(typeof (IDescriptor), namespaceUri, name)
		{
		}
		/// <summary>
		/// Constructs a type descriptor attribute.
		/// </summary>
		/// <param name="contractType">The type of descriptor.</param>
		/// <param name="namespaceUri">The namespace in which the descriptor.</param>
		/// <param name="name">The name of the descriptor.</param>
		protected DescriptorAttribute(Type contractType, string namespaceUri, string name) : base(contractType, namespaceUri, name)
		{
		}
		#endregion
	}
}