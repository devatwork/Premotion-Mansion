using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Implements <see cref="IAutoPopStack{TEntry}"/>.
	/// </summary>
	/// <typeparam name="TEntry">The type of entry on this tack.</typeparam>
	public class AutoPopStack<TEntry> : IAutoPopStack<TEntry>
	{
		#region Implementation of IEnumerable
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<TEntry> GetEnumerator()
		{
			return internalStack.GetEnumerator();
		}
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		#region Implementation of IAutoPopStack<TEntry>
		/// <summary>
		/// Pushes an new entry on the stack.
		/// </summary>
		/// <param name="value">The value of the entry.</param>
		/// <returns>Returns a key to the newly added entry.</returns>
		public IDisposable Push(TEntry value)
		{
			// push the value to the stack
			internalStack.Push(value);

			// return the action which will pop the value from the stack
			return new DisposableAction(() =>
			                            {
			                            	TEntry result;
			                            	internalStack.TryPop(out result);
			                            });
		}
		/// <summary>
		/// Pushes an new entry on the stack.
		/// </summary>
		/// <param name="value">The value of the entry.</param>
		/// <param name="popCallback">This method is invoked when the entry is popped from the stack.</param>
		/// <returns>Returns a key to the newly added entry.</returns>
		public IDisposable Push(TEntry value, Action<TEntry> popCallback)
		{
			// validate arguments
			if (popCallback == null)
				throw new ArgumentNullException("popCallback");

			// push the value to the stack
			internalStack.Push(value);

			// return the action which will pop the value from the stack
			return new DisposableAction(() =>
			                            {
			                            	TEntry result;
			                            	internalStack.TryPop(out result);
			                            	popCallback(result);
			                            });
		}
		/// <summary>
		/// Peeks the top most value.
		/// </summary>
		/// <param name="entry">The top most value when there is one.</param>
		/// <returns>Returns true when the value was peeked from the stack, otherwise false.</returns>
		public bool TryPeek(out TEntry entry)
		{
			return internalStack.TryPeek(out entry);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of entries on this stack.
		/// </summary>
		public int Count
		{
			get { return internalStack.Count; }
		}
		#endregion
		#region Private Fields
		private readonly ConcurrentStack<TEntry> internalStack = new ConcurrentStack<TEntry>();
		#endregion
	}
}