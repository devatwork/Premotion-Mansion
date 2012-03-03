using System;

namespace Premotion.Mansion.Core.Patterns
{
	/// <summary>
	/// This object will execute the action when being disposed.
	/// </summary>
	public class DisposableAction : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// Constructs a disposable action which will execute on dispose.
		/// </summary>
		/// <param name="disposeAction"></param>
		public DisposableAction(Action disposeAction)
		{
			// validate arguments
			if (disposeAction == null)
				throw new ArgumentNullException("disposeAction");

			// set value
			this.disposeAction = disposeAction;
		}
		#endregion
		#region Dispose Implementation
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			// check if the object has been disposed already
			if (disposeManagedResources)
				disposeAction();
		}
		#endregion
		#region Private Fields
		private readonly Action disposeAction;
		#endregion
	}
}