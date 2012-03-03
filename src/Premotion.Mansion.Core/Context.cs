using System;
using System.Collections.Concurrent;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Provides the default implementation of <see cref="IContext"/>.
	/// </summary>
	public class Context : DisposableBase, IContext
	{
		#region Implementation of IContext
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="ContextExtension"/>.</typeparam>
		/// <returns>Returns the extions.</returns>
		/// <exception cref="ContextExtensionNotFoundException">Thrown when the extension is not found.</exception>
		public TContextExtension Extend<TContextExtension>() where TContextExtension : ContextExtension
		{
			ContextExtension extension;
			if (!extensions.TryGetValue(typeof (TContextExtension), out extension))
				throw new ContextExtensionNotFoundException(typeof (TContextExtension));
			return (TContextExtension) extension;
		}
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="ContextExtension"/>.</typeparam>
		/// <param name="factory">The <see cref="Func{IContext,TContextExtension}"/> creating the context when needed.</param>
		/// <returns>Returns the extions.</returns>
		public TContextExtension Extend<TContextExtension>(Func<IContext, TContextExtension> factory) where TContextExtension : ContextExtension
		{
			// validate arguments
			if (factory == null)
				throw new ArgumentNullException("factory");

			return (TContextExtension) extensions.GetOrAdd(typeof (TContextExtension), type => factory(this));
		}
		/// <summary>
		/// Tries to cast this context into another form..
		/// </summary>
		/// <typeparam name="TContext">The type of context, must implement <see cref="IContext"/>.</typeparam>
		/// <returns>Returns context.</returns>
		/// <exception cref="InvalidCastException">Thrown when this context can not be cast into the desired context type.</exception>
		public TContext Cast<TContext>() where TContext : class, IContext
		{
			// check type
			if (!typeof (TContext).IsAssignableFrom(GetType()))
				throw new InvalidCastException(string.Format("Can not cast '{0}' into '{1}'", GetType(), typeof (TContext)));

			// cast
			return this as TContext;
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
			// check for managed resource disposal
			if (!disposeManagedResources)
				return;

			// dispose all the extensions
			foreach (var extension in extensions)
				extension.Value.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly ConcurrentDictionary<Type, ContextExtension> extensions = new ConcurrentDictionary<Type, ContextExtension>();
		#endregion
	}
}