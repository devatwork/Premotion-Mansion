namespace Premotion.Mansion.Core.Patterns.Descriptors
{
	/// <summary>
	/// Represents a descriptor.
	/// </summary>
	public interface IDescriptor
	{
		#region Methods
		/// <summary>
		/// Initializes this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		void Initialize(IMansionContext context);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the properties of this descriptor.
		/// </summary>
		IPropertyBag Properties { get; set; }
		#endregion
	}
}