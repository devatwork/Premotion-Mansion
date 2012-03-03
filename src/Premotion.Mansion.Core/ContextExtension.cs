using System;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Represents an extension of the context.
	/// </summary>
	public abstract class ContextExtension : DisposableBase, IContext
	{
		#region Constructors
		/// <summary>
		/// Constructs a context extesion.
		/// </summary>
		/// <param name="originalContext">The original <see cref="IContext"/> being extended.</param>
		protected ContextExtension(IContext originalContext)
		{
			// validate arguments
			if (originalContext == null)
				throw new ArgumentNullException("originalContext");

			// set values
			this.originalContext = originalContext;
		}
		#endregion
		#region Implementation of IContext
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="ContextExtension"/>.</typeparam>
		/// <returns>Returns the extions.</returns>
		/// <exception cref="ContextExtensionNotFoundException">Thrown when the extension is not found.</exception>
		public TContextExtension Extend<TContextExtension>() where TContextExtension : ContextExtension
		{
			return originalContext.Extend<TContextExtension>();
		}
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="ContextExtension"/>.</typeparam>
		/// <param name="factory">The <see cref="Func{IContext,TContextExtension}"/> creating the context when needed.</param>
		/// <returns>Returns the extions.</returns>
		public TContextExtension Extend<TContextExtension>(Func<IContext, TContextExtension> factory) where TContextExtension : ContextExtension
		{
			return originalContext.Extend(factory);
		}
		/// <summary>
		/// Tries to cast this context into another form..
		/// </summary>
		/// <typeparam name="TContext">The type of context, must implement <see cref="IContext"/>.</typeparam>
		/// <returns>Returns context.</returns>
		/// <exception cref="InvalidCastException">Thrown when this context can not be cast into the desired context type.</exception>
		public TContext Cast<TContext>() where TContext : class, IContext
		{
			return (this as TContext) ?? originalContext.Cast<TContext>();
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
			// might be overriden in derived types
		}
		#endregion
		#region Private Fields
		private readonly IContext originalContext;
		#endregion
	}
}