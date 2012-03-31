using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Base class for all the <see cref="IConfigurableNucleus"/> implementations.
	/// </summary>
	public abstract class NucleusBase : DisposableBase, IConfigurableNucleus
	{
		#region Implementation of INucleus
		/// <summary>
		/// Resolves all the components which implement the <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <returns>Returns an <see cref="IEnumerable{TContract}"/> containing all the matching components.</returns>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		public IEnumerable<TContract> Resolve<TContract>() where TContract : class
		{
			// check if the nucleus is disposed
			CheckDisposed();

			// invoke implementation
			return DoResolve<TContract>().ToList();
		}
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="instance">The instance of <typeparamref name="TContract"/> when found, otherwise null.</param>
		/// <returns>Returns true when a component could be resolved otherwise false.</returns>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		public bool TryResolveSingle<TContract>(out TContract instance) where TContract : class
		{
			// check if the nucleus is disposed
			CheckDisposed();

			// invoke implementation
			return DoTryResolveSingle(out instance);
		}
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="name">The strong name of the compoment which to resolve.</param>
		/// <param name="instance">The instance of <typeparamref name="TContract"/> when found, otherwise null.</param>
		/// <returns>Returns true when a component could be resolved otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when t<paramref name="name"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		public bool TryResolveSingle<TContract>(string name, out TContract instance) where TContract : class
		{
			//validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// check if the nucleus is disposed
			CheckDisposed();

			// invoke implementation
			return DoTryResolveSingle(name, out instance);
		}
		/// <summary>
		/// Resolves all the components which implement the <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <returns>Returns an <see cref="IEnumerable{TContract}"/> containing all the matching components.</returns>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected abstract IEnumerable<TContract> DoResolve<TContract>() where TContract : class;
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="instance">The instance of <typeparamref name="TContract"/> when found, otherwise null.</param>
		/// <returns>Returns true when a component could be resolved otherwise false.</returns>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected abstract bool DoTryResolveSingle<TContract>(out TContract instance) where TContract : class;
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="name">The strong name of the compoment which to resolve.</param>
		/// <param name="instance">The instance of <typeparamref name="TContract"/> when found, otherwise null.</param>
		/// <returns>Returns true when a component could be resolved otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when t<paramref name="name"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected abstract bool DoTryResolveSingle<TContract>(string name, out TContract instance) where TContract : class;
		#endregion
		#region Implementation of IConfigurableNucleus
		/// <summary>
		/// Optimizes the underlying IoC container for performance. The container is now considered read-only.
		/// </summary>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		/// <exception cref="InvalidOperationException">Thrown when the nucleus is in read-only mode.</exception>
		public void Optimize()
		{
			// check if the nucleus is disposed
			CheckDisposed();

			// check readonly
			if (IsReadOnly)
				throw new InvalidOperationException("The Optimize method on this nucleus has been called already.");

			// set the container to read-only
			if (Interlocked.CompareExchange(ref readonlyState, 1, 0) == 0)
				DoOptimize();
		}
		/// <summary>
		/// Registers a <paramref name="instanceFactory"/> for component with contract <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceFactory"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		public void Register<TContract>(Expression<Func<INucleus, TContract>> instanceFactory) where TContract : class
		{
			// validate arguments
			if (instanceFactory == null)
				throw new ArgumentNullException("instanceFactory");

			// check if the nucleus is disposed
			CheckDisposed();

			// check readonly
			if (IsReadOnly)
				throw new InvalidOperationException("The Optimize method on this nucleus has been called already and this nucleus is in read-only state. Make your registration before the Optimze method was invoked.");

			// invoke template method
			DoRegister(instanceFactory);
		}
		/// <summary>
		/// Registers a <paramref name="instanceFactory"/> for component with contract <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="name">The name of the component.</param>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="name"/> or <paramref name="instanceFactory"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		public void Register<TContract>(string name, Expression<Func<INucleus, TContract>> instanceFactory) where TContract : class
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (instanceFactory == null)
				throw new ArgumentNullException("instanceFactory");

			// check if the nucleus is disposed
			CheckDisposed();

			// check readonly
			if (IsReadOnly)
				throw new InvalidOperationException("The Optimize method on this nucleus has been called already and this nucleus is in read-only state. Make your registration before the Optimze method was invoked.");

			// invoke template method
			DoRegister(name, instanceFactory);
		}
		/// <summary>
		/// Registers a <paramref name="instanceFactory"/> for component with contract <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceFactory"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected abstract void DoRegister<TContract>(Expression<Func<INucleus, TContract>> instanceFactory) where TContract : class;
		/// <summary>
		/// Registers a <paramref name="instanceFactory"/> for component with contract <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="name">The name of the component.</param>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="name"/> or <paramref name="instanceFactory"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected abstract void DoRegister<TContract>(string name, Expression<Func<INucleus, TContract>> instanceFactory) where TContract : class;
		/// <summary>
		/// Optimizes the underlying IoC container for performance. The container is now considered read-only.
		/// </summary>
		protected virtual void DoOptimize()
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether the container is read-only or not.
		/// </summary>
		private bool IsReadOnly
		{
			get { return Thread.VolatileRead(ref readonlyState) != 0; }
		}
		#endregion
		#region Private Fields
		private int readonlyState;
		#endregion
	}
}