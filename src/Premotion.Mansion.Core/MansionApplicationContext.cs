using System;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Defines the mansion application context.
	/// </summary>
	public class MansionApplicationContext : MansionContext
	{
		#region Constructors
		/// <summary>
		/// Constructs the mansion application context.
		/// </summary>
		/// <param name="nucleus">The <see cref="IOwnedNuclues"/>.</param>
		public MansionApplicationContext(IOwnedNuclues nucleus) : base(nucleus)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// set values
			nuclues = nucleus;
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			// check for managed resource disposal
			base.DisposeResources(disposeManagedResources);
			if (!disposeManagedResources)
				return;

			// dispose the nucleus
			nuclues.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly IOwnedNuclues nuclues;
		#endregion
	}
}