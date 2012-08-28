using System;
using System.Globalization;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Implements the base class for <see cref="IMansionContext"/> decorators.
	/// </summary>
	public abstract class MansionContextDecorator : DisposableBase, IMansionContext
	{
		#region Constructors
		/// <summary>
		/// Constructs the mansion context decorator.
		/// </summary>
		/// <param name="decoratedContext">The <see cref="IMansionContext"/> being decorated.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="decoratedContext"/> is null.</exception>
		protected MansionContextDecorator(IMansionContext decoratedContext)
		{
			// validate arguments
			if (decoratedContext == null)
				throw new ArgumentNullException("decoratedContext");

			// set values
			this.decoratedContext = decoratedContext;
		}
		#endregion
		#region Implementation of IMansionContext
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="MansionContextExtension"/>.</typeparam>
		/// <returns>Returns the extions.</returns>
		/// <exception cref="MansionContextExtensionNotFoundException">Thrown when the extension is not found.</exception>
		public TContextExtension Extend<TContextExtension>() where TContextExtension : MansionContextExtension
		{
			return decoratedContext.Extend<TContextExtension>();
		}
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="MansionContextExtension"/>.</typeparam>
		/// <param name="factory">The <see cref="Func{IContext,TContextExtension}"/> creating the context when needed.</param>
		/// <returns>Returns the extions.</returns>
		public TContextExtension Extend<TContextExtension>(Func<IMansionContext, TContextExtension> factory) where TContextExtension : MansionContextExtension
		{
			return decoratedContext.Extend(factory);
		}
		/// <summary>
		/// Tries to cast this context into another form..
		/// </summary>
		/// <typeparam name="TContext">The type of context, must implement <see cref="IMansionContext"/>.</typeparam>
		/// <returns>Returns context.</returns>
		/// <exception cref="InvalidCastException">Thrown when this context can not be cast into the desired context type.</exception>
		public TContext Cast<TContext>() where TContext : class, IMansionContext
		{
			return decoratedContext.Cast<TContext>();
		}
		/// <summary>
		/// Gets a flag indicating whether the execution of this script should stop.
		/// </summary>
		public bool HaltExecution
		{
			get { return decoratedContext.HaltExecution; }
		}
		/// <summary>
		/// Breaks all the execution.
		/// </summary>
		public bool BreakExecution
		{
			set { decoratedContext.BreakExecution = value; }
		}
		/// <summary>
		/// Sets a flag indicating whether the execution of the current procedure should be stopped.
		/// </summary>
		public bool BreakTopMostProcedure
		{
			set { decoratedContext.BreakTopMostProcedure = value; }
		}
		/// <summary>
		/// Sets the current user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		public void SetCurrentUserState(UserState authenticatedUser)
		{
			decoratedContext.SetCurrentUserState(authenticatedUser);
		}
		/// <summary>
		/// Sets the frontoffice user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		public void SetFrontofficeUserState(UserState authenticatedUser)
		{
			decoratedContext.SetFrontofficeUserState(authenticatedUser);
		}
		/// <summary>
		/// Sets the backoffice user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		public void SetBackofficeUserState(UserState authenticatedUser)
		{
			decoratedContext.SetBackofficeUserState(authenticatedUser);
		}
		/// <summary>
		/// Gets the stack of this context.
		/// </summary>
		public IAutoPopDictionaryStack<string, IPropertyBag> Stack
		{
			get { return decoratedContext.Stack; }
		}
		/// <summary>
		/// Gets a flag indicating whether this is a front or backoffice contex.t
		/// </summary>
		public bool IsBackoffice
		{
			get { return decoratedContext.IsBackoffice; }
		}
		/// <summary>
		/// Gets the procedure stack.
		/// </summary>
		public IAutoPopDictionaryStack<string, IScript> ProcedureStack
		{
			get { return decoratedContext.ProcedureStack; }
		}
		/// <summary>
		/// Gets/Sets the depth of the execute nested procedures.
		/// </summary>
		public int ExecuteNestedProcedureDepth
		{
			get { return decoratedContext.ExecuteNestedProcedureDepth; }
			set { decoratedContext.ExecuteNestedProcedureDepth = value; }
		}
		/// <summary>
		/// Gets the procedure call stack.
		/// </summary>
		public IAutoPopStack<ScriptTag> ProcedureCallStack
		{
			get { return decoratedContext.ProcedureCallStack; }
		}
		/// <summary>
		/// Gets the event handler stack.
		/// </summary>
		public IAutoPopDictionaryStack<string, IScript> EventHandlerStack
		{
			get { return decoratedContext.EventHandlerStack; }
		}
		/// <summary>
		/// Gets the script stack.
		/// </summary>
		public IAutoPopStack<ITagScript> ScriptStack
		{
			get { return decoratedContext.ScriptStack; }
		}
		/// <summary>
		/// Gets the output pipe stack.
		/// </summary>
		public IAutoPopStack<IOutputPipe> OutputPipeStack
		{
			get { return decoratedContext.OutputPipeStack; }
		}
		/// <summary>
		/// Gets the top most output pipe.
		/// </summary>
		public IOutputPipe OutputPipe
		{
			get { return decoratedContext.OutputPipe; }
		}
		/// <summary>
		/// Gets the input pipe stack.
		/// </summary>
		public IAutoPopStack<IInputPipe> InputPipeStack
		{
			get { return decoratedContext.InputPipeStack; }
		}
		/// <summary>
		/// Gets the top most input pipe.
		/// </summary>
		public IInputPipe InputPipe
		{
			get { return decoratedContext.InputPipe; }
		}
		/// <summary>
		/// Gets the template stack.
		/// </summary>
		public IAutoPopStack<ITemplate> TemplateStack
		{
			get { return decoratedContext.TemplateStack; }
		}
		/// <summary>
		/// Gets the active section stack.
		/// </summary>
		public IAutoPopStack<ActiveSection> ActiveSectionStack
		{
			get { return decoratedContext.ActiveSectionStack; }
		}
		/// <summary>
		/// Gets the <see cref="UserState"/> for the frontoffice.
		/// </summary>
		public UserState FrontofficeUserState
		{
			get { return decoratedContext.FrontofficeUserState; }
		}
		/// <summary>
		/// Gets the <see cref="UserState"/> for the backoffice.
		/// </summary>
		public UserState BackofficeUserState
		{
			get { return decoratedContext.BackofficeUserState; }
		}
		/// <summary>
		/// Gets the user for the current context determined by <see cref="IMansionContext.IsBackoffice"/>
		/// </summary>
		public UserState CurrentUserState
		{
			get { return decoratedContext.CurrentUserState; }
		}
		/// <summary>
		/// Gets the repository stack.
		/// </summary>
		public IAutoPopStack<IRepository> RepositoryStack
		{
			get { return decoratedContext.RepositoryStack; }
		}
		/// <summary>
		/// Gets the top most repository from the stack.
		/// </summary>
		public IRepository Repository
		{
			get { return decoratedContext.Repository; }
		}
		/// <summary>
		/// Gets the <see cref="CultureInfo"/> of the system.
		/// </summary>
		public CultureInfo SystemCulture
		{
			get { return decoratedContext.SystemCulture; }
		}
		/// <summary>
		/// Gets the <see cref="CultureInfo"/> of the user interface.
		/// </summary>
		public CultureInfo UserInterfaceCulture
		{
			get { return decoratedContext.UserInterfaceCulture; }
		}
		/// <summary>
		/// Gets the <see cref="INucleus"/> used by this context.
		/// </summary>
		public INucleus Nucleus
		{
			get { return decoratedContext.Nucleus; }
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
			// do nothing might be overriden
		}
		#endregion
		#region Private Fields
		private readonly IMansionContext decoratedContext;
		#endregion
	}
}