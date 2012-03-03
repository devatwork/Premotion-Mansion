using System;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Represents the context of the current execution.
	/// </summary>
	public interface IContext : IDisposable
	{
		#region Extension Methods
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="ContextExtension"/>.</typeparam>
		/// <returns>Returns the extions.</returns>
		/// <exception cref="ContextExtensionNotFoundException">Thrown when the extension is not found.</exception>
		TContextExtension Extend<TContextExtension>() where TContextExtension : ContextExtension;
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="ContextExtension"/>.</typeparam>
		/// <param name="factory">The <see cref="Func{IContext,TContextExtension}"/> creating the context when needed.</param>
		/// <returns>Returns the extions.</returns>
		TContextExtension Extend<TContextExtension>(Func<IContext, TContextExtension> factory) where TContextExtension : ContextExtension;
		#endregion
		#region Cast Methods
		/// <summary>
		/// Tries to cast this context into another form..
		/// </summary>
		/// <typeparam name="TContext">The type of context, must implement <see cref="IContext"/>.</typeparam>
		/// <returns>Returns context.</returns>
		/// <exception cref="InvalidCastException">Thrown when this context can not be cast into the desired context type.</exception>
		TContext Cast<TContext>() where TContext : class, IContext;
		#endregion
	}
}