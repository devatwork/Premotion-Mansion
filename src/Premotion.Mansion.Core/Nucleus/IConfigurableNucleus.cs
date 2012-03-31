using System;
using System.Linq.Expressions;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Allows configuration of the <see cref="INucleus"/>.
	/// </summary>
	public interface IConfigurableNucleus : INucleus, IDisposable
	{
		#region Register Methods
		/// <summary>
		/// Registers a <paramref name="instanceFactory"/> for component with contract <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceFactory"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		void Register<TContract>(Expression<Func<INucleus, TContract>> instanceFactory) where TContract : class;
		/// <summary>
		/// Registers a <paramref name="instanceFactory"/> for component with contract <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="name">The name of the component.</param>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="name"/> or <paramref name="instanceFactory"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		void Register<TContract>(string name, Expression<Func<INucleus, TContract>> instanceFactory) where TContract : class;
		#endregion
		#region Optimize Methods
		/// <summary>
		/// Optimizes the underlying IoC container for performance. The container is now considered read-only.
		/// </summary>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		/// <exception cref="InvalidOperationException">Thrown when the nucleus is in read-only mode.</exception>
		void Optimize();
		#endregion
	}
}