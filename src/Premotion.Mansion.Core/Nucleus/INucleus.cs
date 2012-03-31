using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Provides Inversion of Control capabilities.
	/// </summary>
	public interface INucleus
	{
		#region Component Methods
		/// <summary>
		/// Resolves all the components which implement the <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <returns>Returns an <see cref="IEnumerable{TContract}"/> containing all the matching components.</returns>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		IEnumerable<TContract> Resolve<TContract>() where TContract : class;
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="instance">The instance of <typeparamref name="TContract"/> when found, otherwise null.</param>
		/// <returns>Returns true when a component could be resolved otherwise false.</returns>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		bool TryResolveSingle<TContract>(out TContract instance) where TContract : class;
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="name">The strong name of the compoment which to resolve.</param>
		/// <param name="instance">The instance of <typeparamref name="TContract"/> when found, otherwise null.</param>
		/// <returns>Returns true when a component could be resolved otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when t<paramref name="name"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		bool TryResolveSingle<TContract>(string name, out TContract instance) where TContract : class;
		#endregion
	}
}