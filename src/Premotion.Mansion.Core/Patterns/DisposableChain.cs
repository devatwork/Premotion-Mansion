using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Patterns
{
	/// <summary>
	/// Represents a chain of disposable objects.
	/// </summary>
	public class DisposableChain : DisposableBase
	{
		#region Add Methods
		/// <summary>
		/// Adds <paramref name="disposable"/> to this disposable chain.
		/// </summary>
		/// <param name="disposable">The disposable.</param>
		/// <returns>Returns this chain.</returns>
		public DisposableChain Add(IDisposable disposable)
		{
			// validate arguments
			if (disposable == null)
				throw new ArgumentNullException("disposable");
			stack.Push(disposable);
			return this;
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
			if (!disposeManagedResources)
				return;
			foreach (var disposable in stack)
				disposable.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly Stack<IDisposable> stack = new Stack<IDisposable>();
		#endregion
	}
}