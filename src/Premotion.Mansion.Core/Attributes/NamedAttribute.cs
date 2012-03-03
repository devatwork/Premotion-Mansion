using System;

namespace Premotion.Mansion.Core.Attributes
{
	/// <summary>
	/// Marks any type as exported indicating by its fully qualified domain name.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class NamedAttribute : ExportedAttribute
	{
		#region Constructors
		/// <summary>
		/// Constructs an identifier.
		/// </summary>
		/// <param name="namespaceUri">The namespace of the identifier.</param>
		/// <param name="name">The name of the identifier.</param>
		public NamedAttribute(string namespaceUri, string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(namespaceUri))
				throw new ArgumentNullException("namespaceUri");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set values
			NamespaceUri = namespaceUri;
			Name = name;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the namespace uri.
		/// </summary>
		public string NamespaceUri { get; private set; }
		#endregion
	}
}