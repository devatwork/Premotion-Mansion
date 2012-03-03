using System;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle
{
	/// <summary>
	/// Base class for services implementing <see cref="IManagedLifecycleService"/>.
	/// </summary>
	public class ManagedLifecycleService : DisposableBase, IManagedLifecycleService
	{
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			// do nothing, might be overriden in inherinting types
		}
		#endregion
		#region Implementation of IManagedLifecycleService
		/// <summary>
		/// Invoked just before this service is used for the first time.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		public void Start(INucleusAwareContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

			// invoke template method
			DoStart(context);
		}
		/// <summary>
		/// Invoked just before this service is used for the first time.
		/// </summary>
		/// <param name="context">The <see cref="NucleusContext"/>.</param>
		protected virtual void DoStart(INucleusAwareContext context)
		{
			// do nothing, might be overriden in inherinting types
		}
		#endregion
	}
}