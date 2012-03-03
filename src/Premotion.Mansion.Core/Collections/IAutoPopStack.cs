using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Represents a stack which will pop auomatically..
	/// </summary>
	/// <typeparam name="TEntry">The type of entries which can be pushed on the stack.</typeparam>
	public interface IAutoPopStack<TEntry> : IEnumerable<TEntry>
	{
		#region Stack Methods
		/// <summary>
		/// Pushes an new entry on the stack.
		/// </summary>
		/// <param name="value">The value of the entry.</param>
		/// <returns>Returns a key to the newly added entry.</returns>
		IDisposable Push(TEntry value);
		/// <summary>
		/// Pushes an new entry on the stack.
		/// </summary>
		/// <param name="value">The value of the entry.</param>
		/// <param name="popCallback">This method is invoked when the entry is popped from the stack.</param>
		/// <returns>Returns a key to the newly added entry.</returns>
		IDisposable Push(TEntry value, Action<TEntry> popCallback);
		/// <summary>
		/// Peeks the top most value.
		/// </summary>
		/// <param name="entry">The top most value when there is one.</param>
		/// <returns>Returns true when the value was peeked from the stack, otherwise false.</returns>
		bool TryPeek(out TEntry entry);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of entries on this stack.
		/// </summary>
		int Count { get; }
		#endregion
	}
}