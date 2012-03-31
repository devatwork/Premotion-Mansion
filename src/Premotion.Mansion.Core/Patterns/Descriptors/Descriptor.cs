using System;

namespace Premotion.Mansion.Core.Patterns.Descriptors
{
	/// <summary>
	/// Provides a default implementation for <see cref="IDescriptor"/>.
	/// </summary>
	public abstract class Descriptor : IDescriptor
	{
		#region Methods
		/// <summary>
		/// Initializes this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void Initialize(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			DoInitialize(context);
		}

		/// <summary>
		/// Initializes this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected virtual void DoInitialize(IMansionContext context)
		{
			// do nothin
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the properties of this descriptor.
		/// </summary>
		public IPropertyBag Properties { get; set; }
		#endregion
	}
}