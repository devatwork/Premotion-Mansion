using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Represents a group of stack keys.
	/// </summary>
	public class AutoStackKeyGroup : DisposableBase
	{
		#region Add Methods
		/// <summary>
		/// Adds a stack key to this group.
		/// </summary>
		/// <param name="key">The key which to add.</param>
		public void Push(IDisposable key)
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");

			internalStack.Push(key);
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

			// dispose all the keys
			foreach (var key in internalStack)
				key.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly Stack<IDisposable> internalStack = new Stack<IDisposable>();
		#endregion
	}
}