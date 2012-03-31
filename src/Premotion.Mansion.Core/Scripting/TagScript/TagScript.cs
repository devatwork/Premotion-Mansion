using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Implements a script using tags.
	/// </summary>
	public class TagScript : DisposableBase, ITagScript
	{
		#region Constructors
		/// <summary>
		/// Constructs a tag script.
		/// </summary>
		/// <param name="documentTag">The document tag.</param>
		public TagScript(ScriptTag documentTag)
		{
			// validate arguments
			if (documentTag == null)
				throw new ArgumentNullException("documentTag");

			// set values
			DocumentTag = documentTag;
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
			if (!disposeManagedResources)
				return;

			// loop through all the disposables and dispose them
			foreach (var disposable in disposables)
				disposable.Dispose();
		}
		#endregion
		#region Implementation of IScript
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		public void Execute(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

			// delegate execution to document tag
			DocumentTag.Execute(context);
		}
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <typeparam name="TResult">The result type.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the result of this script expression.</returns>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		public TResult Execute<TResult>(IMansionContext context)
		{
			throw new NotSupportedException();
		}
		#endregion
		#region Implementation of ITagScript
		/// <summary>
		/// Initializes this script.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void Initialize(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

			DocumentTag.InitializeContext(context, this);
		}
		/// <summary>
		/// Registers a procedure.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="procedureName">The name of the procedure which to register.</param>
		/// <param name="tag">The script tag implementing the procedure.</param>
		public void RegisterProcedure(IMansionContext context, string procedureName, ScriptTag tag)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(procedureName))
				throw new ArgumentNullException("procedureName");
			if (tag == null)
				throw new ArgumentNullException("tag");
			CheckDisposed();

			// get the script context extension
			disposables.Add(context.ProcedureStack.Push(procedureName, tag, false));
		}
		/// <summary>
		/// Registers an event handler.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="eventName">The name of the event to which the handler is attached.</param>
		/// <param name="tag">The script tag implementing the procedure.</param>
		public void RegisterEventHandler(IMansionContext context, string eventName, ScriptTag tag)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(eventName))
				throw new ArgumentNullException("eventName");
			if (tag == null)
				throw new ArgumentNullException("tag");
			CheckDisposed();

			// get the script context extension
			disposables.Add(context.EventHandlerStack.Push(eventName, tag, false));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the document tag of this script.
		/// </summary>
		private ScriptTag DocumentTag { get; set; }
		#endregion
		#region Private Fields
		private readonly ICollection<IDisposable> disposables = new List<IDisposable>();
		#endregion
	}
}