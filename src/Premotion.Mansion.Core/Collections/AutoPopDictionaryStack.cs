using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Implements <see cref="IAutoPopDictionaryStack{TKey,TEntry}"/>.
	/// </summary>
	/// <typeparam name="TKey">The type of keys which can be pushed on the stack.</typeparam>
	/// <typeparam name="TEntry">The type of entries which can be pushed on the stack.</typeparam>
	public class AutoPopDictionaryStack<TKey, TEntry> : IAutoPopDictionaryStack<TKey, TEntry> where TKey : class where TEntry : class
	{
		#region Constructors
		/// <summary>
		/// Constructs a dictionary auto stack.
		/// </summary>
		/// <param name="comparer">The equality comparer.</param>
		public AutoPopDictionaryStack(IEqualityComparer<TKey> comparer)
		{
			// validate arguments
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			// set values
			internalStack = new ConcurrentDictionary<TKey, ConcurrentStack<TEntry>>(comparer);
		}
		/// <summary>
		/// Constructs a dictionary auto stack.
		/// </summary>
		/// <param name="comparer">The equality comparer.</param>
		/// <param name="initialStack">The initial stack values.</param>
		public AutoPopDictionaryStack(IEqualityComparer<TKey> comparer, IEnumerable<KeyValuePair<TKey, IEnumerable<TEntry>>> initialStack)
		{
			// validate arguments
			if (comparer == null)
				throw new ArgumentNullException("comparer");
			if (initialStack == null)
				throw new ArgumentNullException("initialStack");

			// set values
			internalStack = new ConcurrentDictionary<TKey, ConcurrentStack<TEntry>>(comparer);

			// copy initial values
			foreach (var entry in initialStack)
				internalStack.TryAdd(entry.Key, new ConcurrentStack<TEntry>(entry.Value));
		}
		#endregion
		#region Implementation of IAutoPopDictionaryStack<TKey,TEntry>
		/// <summary>
		/// Pushes an new entry on the stack in the target dataspace.
		/// </summary>
		/// <param name="key">The dataspace to which the new entry will be added.</param>
		/// <param name="value">The value of the entry.</param>
		/// <param name="isGlobal">Flag indicating whether the entry is global or not.</param>
		/// <returns>Returns a key to the newly added entry.</returns>
		public IDisposable Push(TKey key, TEntry value, bool isGlobal)
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");
			if (value == null)
				throw new ArgumentNullException("value");

			// get the stack
			var pushStack = internalStack.GetOrAdd(key, k => new ConcurrentStack<TEntry>());

			// push the value to the stack
			pushStack.Push(value);

			// return the disposable action
			return new DisposableAction(() =>
			                            {
			                            	// check if the entry is global
			                            	if (isGlobal)
			                            		return;

			                            	// get the stack
			                            	ConcurrentStack<TEntry> popStack;
			                            	if (!internalStack.TryGetValue(key, out popStack))
			                            		return;

			                            	// pop the value from the stack
			                            	TEntry entry;
			                            	popStack.TryPop(out entry);
			                            });
		}
		/// <summary>
		/// Peeks the top most value for the specified dataspace.
		/// </summary>
		/// <typeparam name="TValue">The type of the return value.</typeparam>
		/// <param name="key">The dataspace.</param>
		/// <returns>Returns <typeparamref name="TValue"/>.</returns>
		/// <exception cref="DataspaceNotFoundException">Thrown when the dataspace identified by <paramref name="key"/> could not be found or when the dataspace can not be converted to <typeparamref name="TValue"/>.</exception>
		public TValue Peek<TValue>(TKey key) where TValue : TEntry
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");

			// get the stack for this entry
			ConcurrentStack<TEntry> stack;
			if (!internalStack.TryGetValue(key, out stack))
				throw new DataspaceNotFoundException(key.ToString(), typeof (TValue));

			// get the value from the stack
			TEntry stackValue;
			if (! stack.TryPeek(out stackValue))
				throw new DataspaceNotFoundException(key.ToString(), typeof (TValue));

			// cast the value
			return (TValue) stackValue;
		}
		/// <summary>
		/// Peeks the all the values for the specified dataspace.
		/// </summary>
		/// <param name="key">The dataspace.</param>
		/// <returns>Returns all the values of <paramref name="key"/>.</returns>
		public IEnumerable<TEntry> PeekAll(TKey key)
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");

			// get the stack for this entry
			ConcurrentStack<TEntry> stack;
			if (!internalStack.TryGetValue(key, out stack))
				yield break;

			// return all the values on the stack
			foreach (var value in stack)
				yield return value;
		}
		/// <summary>
		/// Peeks the top most value for the specified dataspace.
		/// </summary>
		/// <param name="key">The dataspace.</param>
		/// <param name="value">The top most value.</param>
		/// <returns>Returns true when an entry was foun for the <paramref name="key"/>, otherwise false.</returns>
		public bool TryPeek(TKey key, out TEntry value)
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");

			// get the stack for this entry
			ConcurrentStack<TEntry> stack;
			if (!internalStack.TryGetValue(key, out stack))
			{
				value = default(TEntry);
				return false;
			}

			// get the value from the stack
			return stack.TryPeek(out value);
		}
		/// <summary>
		/// Peeks the top most value for the specified dataspace.
		/// </summary>
		/// <typeparam name="TValue">The type of the return value.</typeparam>
		/// <param name="key">The dataspace.</param>
		/// <param name="value">The top most value.</param>
		/// <returns>Returns true when an entry was foun for the <paramref name="key"/>, otherwise false.</returns>
		public bool TryPeek<TValue>(TKey key, out TValue value) where TValue : TEntry
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");

			// get the stack for this entry
			ConcurrentStack<TEntry> stack;
			if (!internalStack.TryGetValue(key, out stack))
			{
				value = default(TValue);
				return false;
			}

			// get the value from the stack
			TEntry stackValue;
			if (! stack.TryPeek(out stackValue))
			{
				value = default(TValue);
				return false;
			}

			// cast the value
			value = (TValue) stackValue;
			return true;
		}
		/// <summary>
		/// Pops the dataspace with key <paramref name="key"/> completely from the stack.
		/// </summary>
		/// <param name="key">The key of the dataspace.</param>
		public void PopDataspace(TKey key)
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");

			ConcurrentStack<TEntry> stack;
			internalStack.TryRemove(key, out stack);
		}
		#endregion
		#region Implementation of IEnumerable
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<KeyValuePair<TKey, IEnumerable<TEntry>>> GetEnumerator()
		{
			return internalStack.Select(entry => new KeyValuePair<TKey, IEnumerable<TEntry>>(entry.Key, entry.Value)).GetEnumerator();
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
		#region Private Fields
		private readonly ConcurrentDictionary<TKey, ConcurrentStack<TEntry>> internalStack;
		#endregion
	}
}