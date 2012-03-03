using System;

namespace Premotion.Mansion.Core.Patterns.Descriptors
{
	/// <summary>
	/// Provides a default implementation for <see cref="IDescriptor"/>.
	/// </summary>
	public abstract class Descriptor : IDescriptor
	{
		#region Constructors
		/// <summary>
		/// Constructs a descriptor.
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		protected Descriptor(string namespaceUri, string name, IPropertyBag properties)
		{
			// validate arguments
			if (string.IsNullOrEmpty(namespaceUri))
				throw new ArgumentNullException("namespaceUri");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// set values
			NamespaceUri = namespaceUri;
			Name = name;
			Properties = properties;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the namespace of this descriptor.
		/// </summary>
		public string NamespaceUri { get; private set; }
		/// <summary>
		/// Gets the name of this descriptor.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the properties of this descriptor.
		/// </summary>
		public virtual IPropertyBag Properties { get; private set; }
		#endregion
	}
}