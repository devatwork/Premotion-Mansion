using System;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Implements the base class of all type descriptors.
	/// </summary>
	public abstract class TypeDescriptor : Descriptor
	{
		#region Constructors
		/// <summary>
		/// Constructs a descriptor.
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		protected TypeDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties)
		{
			// validate arguments
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");

			// set values
			TypeDefinition = typeDefinition;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="ITypeDefinition"/> to which this descriptor is applied.
		/// </summary>
		public ITypeDefinition TypeDefinition { get; private set; }
		#endregion
	}
}