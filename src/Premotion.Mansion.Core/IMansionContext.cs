using System;
using System.Globalization;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Represents the context of the current execution.
	/// </summary>
	public interface IMansionContext
	{
		#region Extension Methods
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="MansionContextExtension"/>.</typeparam>
		/// <returns>Returns the extions.</returns>
		/// <exception cref="MansionContextExtensionNotFoundException">Thrown when the extension is not found.</exception>
		TContextExtension Extend<TContextExtension>() where TContextExtension : MansionContextExtension;
		/// <summary>
		/// Extends this context.
		/// </summary>
		/// <typeparam name="TContextExtension">The type of extension, must have a default constructor and inherit from <see cref="MansionContextExtension"/>.</typeparam>
		/// <param name="factory">The <see cref="Func{IContext,TContextExtension}"/> creating the context when needed.</param>
		/// <returns>Returns the extions.</returns>
		TContextExtension Extend<TContextExtension>(Func<IMansionContext, TContextExtension> factory) where TContextExtension : MansionContextExtension;
		#endregion
		#region Cast Methods
		/// <summary>
		/// Tries to cast this context into another form..
		/// </summary>
		/// <typeparam name="TContext">The type of context, must implement <see cref="IMansionContext"/>.</typeparam>
		/// <returns>Returns context.</returns>
		/// <exception cref="InvalidCastException">Thrown when this context can not be cast into the desired context type.</exception>
		TContext Cast<TContext>() where TContext : class, IMansionContext;
		#endregion
		#region Control Flow Properties
		/// <summary>
		/// Gets a flag indicating whether the execution of this script should stop.
		/// </summary>
		bool HaltExecution { get; }
		/// <summary>
		/// Breaks all the execution.
		/// </summary>
		bool BreakExecution { set; }
		/// <summary>
		/// Sets a flag indicating whether the execution of the current procedure should be stopped.
		/// </summary>
		bool BreakTopMostProcedure { set; }
		#endregion
		#region User Methods
		/// <summary>
		/// Sets the current user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		void SetCurrentUserState(UserState authenticatedUser);
		/// <summary>
		/// Sets the frontoffice user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		void SetFrontofficeUserState(UserState authenticatedUser);
		/// <summary>
		/// Sets the backoffice user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		void SetBackofficeUserState(UserState authenticatedUser);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the stack of this context.
		/// </summary>
		IAutoPopDictionaryStack<string, IPropertyBag> Stack { get; }
		/// <summary>
		/// Gets a flag indicating whether this is a front or backoffice contex.t
		/// </summary>
		bool IsBackoffice { get; }
		/// <summary>
		/// Gets the procedure stack.
		/// </summary>
		IAutoPopDictionaryStack<string, IScript> ProcedureStack { get; }
		/// <summary>
		/// Gets/Sets the depth of the execute nested procedures.
		/// </summary>
		int ExecuteNestedProcedureDepth { get; set; }
		/// <summary>
		/// Gets the procedure call stack.
		/// </summary>
		IAutoPopStack<ScriptTag> ProcedureCallStack { get; }
		/// <summary>
		/// Gets the event handler stack.
		/// </summary>
		IAutoPopDictionaryStack<string, IScript> EventHandlerStack { get; }
		/// <summary>
		/// Gets the script stack.
		/// </summary>
		IAutoPopStack<ITagScript> ScriptStack { get; }
		/// <summary>
		/// Gets the output pipe stack.
		/// </summary>
		IAutoPopStack<IOutputPipe> OutputPipeStack { get; }
		/// <summary>
		/// Gets the top most output pipe.
		/// </summary>
		IOutputPipe OutputPipe { get; }
		/// <summary>
		/// Gets the input pipe stack.
		/// </summary>
		IAutoPopStack<IInputPipe> InputPipeStack { get; }
		/// <summary>
		/// Gets the top most input pipe.
		/// </summary>
		IInputPipe InputPipe { get; }
		/// <summary>
		/// Gets the template stack.
		/// </summary>
		IAutoPopStack<ITemplate> TemplateStack { get; }
		/// <summary>
		/// Gets the active section stack.
		/// </summary>
		IAutoPopStack<ActiveSection> ActiveSectionStack { get; }
		/// <summary>
		/// Gets the <see cref="UserState"/> for the frontoffice.
		/// </summary>
		UserState FrontofficeUserState { get; }
		/// <summary>
		/// Gets the <see cref="UserState"/> for the backoffice.
		/// </summary>
		UserState BackofficeUserState { get; }
		/// <summary>
		/// Gets the user for the current context determined by <see cref="IsBackoffice"/>
		/// </summary>
		UserState CurrentUserState { get; }
		/// <summary>
		/// Gets the repository stack.
		/// </summary>
		IAutoPopStack<IRepository> RepositoryStack { get; }
		/// <summary>
		/// Gets the top most repository from the stack.
		/// </summary>
		IRepository Repository { get; }
		/// <summary>
		/// Gets the <see cref="CultureInfo"/> of the system.
		/// </summary>
		CultureInfo SystemCulture { get; }
		/// <summary>
		/// Gets the <see cref="CultureInfo"/> of the user interface.
		/// </summary>
		CultureInfo UserInterfaceCulture { get; }
		/// <summary>
		/// Gets the <see cref="INucleus"/> used by this context.
		/// </summary>
		INucleus Nucleus { get; }
		#endregion
	}
}