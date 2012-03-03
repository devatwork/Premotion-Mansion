using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Represents a dictionary stack which will pop auomatically.
	/// </summary>
	/// <typeparam name="TKey">The type of keys which can be pushed on the stack.</typeparam>
	/// <typeparam name="TEntry">The type of entries which can be pushed on the stack.</typeparam>
	public interface IAutoPopDictionaryStack<TKey, TEntry> : IEnumerable<KeyValuePair<TKey, IEnumerable<TEntry>>> where TKey : class where TEntry : class
	{
		#region Stack Methods
		/// <summary>
		/// Pushes an new entry on the stack in the target dataspace.
		/// </summary>
		/// <param name="key">The dataspace to which the new entry will be added.</param>
		/// <param name="value">The value of the entry.</param>
		/// <param name="isGlobal">Flag indicating whether the entry is global or not.</param>
		/// <returns>Returns a key to the newly added entry.</returns>
		IDisposable Push(TKey key, TEntry value, bool isGlobal = false);
		/// <summary>
		/// Peeks the top most value for the specified dataspace.
		/// </summary>
		/// <typeparam name="TValue">The type of the return value.</typeparam>
		/// <param name="key">The dataspace.</param>
		/// <returns>Returns <typeparamref name="TValue"/>.</returns>
		/// <exception cref="DataspaceNotFoundException">Thrown when the dataspace identified by <paramref name="key"/> could not be found or when the dataspace can not be converted to <typeparamref name="TValue"/>.</exception>
		TValue Peek<TValue>(TKey key) where TValue : TEntry;
		/// <summary>
		/// Peeks the all the values for the specified dataspace.
		/// </summary>
		/// <param name="key">The dataspace.</param>
		/// <returns>Returns all the values of <paramref name="key"/>.</returns>
		IEnumerable<TEntry> PeekAll(TKey key);
		/// <summary>
		/// Peeks the top most value for the specified dataspace.
		/// </summary>
		/// <param name="key">The dataspace.</param>
		/// <param name="value">The top most value.</param>
		/// <returns>Returns true when an entry was foun for the <paramref name="key"/>, otherwise false.</returns>
		bool TryPeek(TKey key, out TEntry value);
		/// <summary>
		/// Peeks the top most value for the specified dataspace.
		/// </summary>
		/// <typeparam name="TValue">The type of the return value.</typeparam>
		/// <param name="key">The dataspace.</param>
		/// <param name="value">The top most value.</param>
		/// <returns>Returns true when an entry was foun for the <paramref name="key"/>, otherwise false.</returns>
		bool TryPeek<TValue>(TKey key, out TValue value) where TValue : TEntry;
		/// <summary>
		/// Pops the dataspace with key <paramref name="key"/> completely from the stack.
		/// </summary>
		/// <param name="key">The key of the dataspace.</param>
		void PopDataspace(TKey key);
		#endregion
	}
}