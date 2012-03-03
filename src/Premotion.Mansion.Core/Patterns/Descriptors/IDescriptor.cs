namespace Premotion.Mansion.Core.Patterns.Descriptors
{
	/// <summary>
	/// Represents a descriptor.
	/// </summary>
	public interface IDescriptor
	{
		#region Properties
		/// <summary>
		/// Gets the namespace of this descriptor.
		/// </summary>
		string NamespaceUri { get; }
		/// <summary>
		/// Gets the name of this descriptor.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Gets the properties of this descriptor.
		/// </summary>
		IPropertyBag Properties { get; }
		#endregion
	}
}