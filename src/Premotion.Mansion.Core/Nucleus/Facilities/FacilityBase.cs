using System;
using Premotion.Mansion.Core.Nucleus.Extension;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Nucleus.Facilities
{
	/// <summary>
	/// Implements the base class for type implementing <see cref="IFacility"/>.
	/// </summary>
	public abstract class FacilityBase : DisposableBase, IFacility
	{
		#region Implementation of IFacility
		/// <summary>
		/// Activates this facility in the <paramref name="nucleus"/>. This event it typically used to register listeners to nucleus events.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="nucleus">The <see cref="IExtendedNucleus"/> in which this facility is activated.</param>
		public void Activate(IContext context, IExtendedNucleus nucleus)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");
			CheckDisposed();

			// invoke template method
			DoActivate(context, nucleus);
		}
		/// <summary>
		/// Activates this facility in the <paramref name="nucleus"/>. This event it typically used to register listeners to nucleus events.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="nucleus">The <see cref="IExtendedNucleus"/> in which this facility is activated.</param>
		protected abstract void DoActivate(IContext context, IExtendedNucleus nucleus);
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			// do nothing
		}
		#endregion
	}
}