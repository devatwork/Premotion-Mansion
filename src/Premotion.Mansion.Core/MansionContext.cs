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
	/// Represents the context for mansion applications.
	/// </summary>
	public class MansionContext : Context, INucleusAwareContext
	{
		#region Constructors
		/// <summary>
		/// Constructs a context extesion.
		/// </summary>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		public MansionContext(INucleus nucleus)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// get the nucleus
			this.nucleus = nucleus;
			SystemCulture = CultureInfo.InvariantCulture;
			UserInterfaceCulture = CultureInfo.CurrentUICulture;
		}
		#endregion
		#region Repository Methods
		/// <summary>
		/// Gets the unwrapped <see cref="IRepository"/> which is stripped from all decorators.
		/// </summary>
		/// <returns>Returns the unwrapped <see cref="IRepository"/>.</returns>
		public IRepository GetUnwrappedRepository()
		{
			// get the current repository
			var repository = Repository;

			// unwrap the decorators
			while (repository is IRepositoryDecorator)
			{
				// unwrap it
				repository = ((IRepositoryDecorator) repository).DecoratedRepository;
			}

			return repository;
		}
		#endregion
		#region Control Flow Properties
		/// <summary>
		/// Gets a flag indicating whether the execution of this script should stop.
		/// </summary>
		public bool HaltExecution
		{
			get { return BreakTopMostProcedure || BreakExecution; }
		}
		/// <summary>
		/// Breaks all the execution.
		/// </summary>
		public bool BreakExecution { private get; set; }
		/// <summary>
		/// Sets a flag indicating whether the execution of the current procedure should be stopped.
		/// </summary>
		public bool BreakTopMostProcedure { private get; set; }
		#endregion
		#region User Methods
		/// <summary>
		/// Sets the current user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		public void SetCurrentUserState(UserState authenticatedUser)
		{
			// validate arguments
			if (authenticatedUser == null)
				throw new ArgumentNullException("authenticatedUser");

			// set the proper user
			if (IsBackoffice)
				backofficeUser = authenticatedUser;
			else
				frontofficeUser = authenticatedUser;
		}
		/// <summary>
		/// Sets the frontoffice user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		public void SetFrontofficeUserState(UserState authenticatedUser)
		{
			// validate arguments
			if (authenticatedUser == null)
				throw new ArgumentNullException("authenticatedUser");
			frontofficeUser = authenticatedUser;
		}
		/// <summary>
		/// Sets the backoffice user.
		/// </summary>
		/// <param name="authenticatedUser">The authenticated user.</param>
		public void SetBackofficeUserState(UserState authenticatedUser)
		{
			// validate arguments
			if (authenticatedUser == null)
				throw new ArgumentNullException("authenticatedUser");
			backofficeUser = authenticatedUser;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the stack of this context.
		/// </summary>
		public IAutoPopDictionaryStack<string, IPropertyBag> Stack
		{
			get { return stack; }
			protected set { stack = value; }
		}
		/// <summary>
		/// Gets a flag indicating whether this is a front or backoffice contex.t
		/// </summary>
		public bool IsBackoffice { get; protected set; }
		/// <summary>
		/// Gets the procedure stack.
		/// </summary>
		public IAutoPopDictionaryStack<string, IScript> ProcedureStack
		{
			get { return procedureStack; }
		}
		/// <summary>
		/// Gets the event handler stack.
		/// </summary>
		public IAutoPopDictionaryStack<string, IScript> EventHandlerStack
		{
			get { return eventHandlerStack; }
		}
		/// <summary>
		/// Gets the script stack.
		/// </summary>
		public IAutoPopStack<ITagScript> ScriptStack
		{
			get { return scriptStack; }
		}
		/// <summary>
		/// Gets the output pipe stack.
		/// </summary>
		public IAutoPopStack<IOutputPipe> OutputPipeStack
		{
			get { return outputPipeStack; }
		}
		/// <summary>
		/// Gets the top most output pipe.
		/// </summary>
		public IOutputPipe OutputPipe
		{
			get
			{
				IOutputPipe pipe;
				if (!OutputPipeStack.TryPeek(out pipe))
					throw new InvalidOperationException("There is no output pipe on the stack.");
				return pipe;
			}
		}
		/// <summary>
		/// Gets the input pipe stack.
		/// </summary>
		public IAutoPopStack<IInputPipe> InputPipeStack
		{
			get { return inputPipeStack; }
		}
		/// <summary>
		/// Gets the top most input pipe.
		/// </summary>
		public IInputPipe InputPipe
		{
			get
			{
				IInputPipe pipe;
				if (!InputPipeStack.TryPeek(out pipe))
					throw new InvalidOperationException("There is no input pipe on the stack.");
				return pipe;
			}
		}
		/// <summary>
		/// Gets the template stack.
		/// </summary>
		public IAutoPopStack<ITemplate> TemplateStack
		{
			get { return templateStack; }
		}
		/// <summary>
		/// Gets the active section stack.
		/// </summary>
		public IAutoPopStack<ActiveSection> ActiveSectionStack
		{
			get { return activeSectionStack; }
		}
		/// <summary>
		/// Gets the <see cref="UserState"/> for the frontoffice.
		/// </summary>
		public UserState FrontofficeUserState
		{
			get
			{
				// make sure a user is specified
				if (frontofficeUser == null)
					throw new InvalidOperationException("The security context was not initialized correctly. frontoffice user is missing");
				return frontofficeUser;
			}
		}
		/// <summary>
		/// Gets the <see cref="UserState"/> for the backoffice.
		/// </summary>
		public UserState BackofficeUserState
		{
			get
			{
				// make sure a user is specified
				if (backofficeUser == null)
					throw new InvalidOperationException("The security context was not initialized correctly. backoffice user is missing");
				return backofficeUser;
			}
		}
		/// <summary>
		/// Gets the user for the current context determined by <see cref="IsBackoffice"/>
		/// </summary>
		public UserState CurrentUserState
		{
			get { return IsBackoffice ? BackofficeUserState : FrontofficeUserState; }
		}
		/// <summary>
		/// Gets the repository stack.
		/// </summary>
		public IAutoPopStack<IRepository> RepositoryStack
		{
			get { return repositoryStack; }
		}
		/// <summary>
		/// Gets the top most repository from the stack.
		/// </summary>
		public IRepository Repository
		{
			get
			{
				IRepository repository;
				if (!RepositoryStack.TryPeek(out repository))
					throw new InvalidOperationException("No repository found on the stack. Please open repository first.");
				return repository;
			}
		}
		/// <summary>
		/// Gets the <see cref="CultureInfo"/> of the system.
		/// </summary>
		public CultureInfo SystemCulture { get; private set; }
		/// <summary>
		/// Gets the <see cref="CultureInfo"/> of the user interface.
		/// </summary>
		public CultureInfo UserInterfaceCulture { get; private set; }
		/// <summary>
		/// Gets the <see cref="INucleus"/> used by this context.
		/// </summary>
		public INucleus Nucleus
		{
			get { return nucleus; }
		}
		#endregion
		#region Private fields
		private readonly IAutoPopStack<ActiveSection> activeSectionStack = new AutoPopStack<ActiveSection>();
		private readonly IAutoPopDictionaryStack<string, IScript> eventHandlerStack = new AutoPopDictionaryStack<string, IScript>(StringComparer.OrdinalIgnoreCase);
		private readonly IAutoPopStack<IInputPipe> inputPipeStack = new AutoPopStack<IInputPipe>();
		private readonly INucleus nucleus;
		private readonly IAutoPopStack<IOutputPipe> outputPipeStack = new AutoPopStack<IOutputPipe>();
		private readonly IAutoPopDictionaryStack<string, IScript> procedureStack = new AutoPopDictionaryStack<string, IScript>(StringComparer.OrdinalIgnoreCase);
		private readonly IAutoPopStack<IRepository> repositoryStack = new AutoPopStack<IRepository>();
		private readonly IAutoPopStack<ITagScript> scriptStack = new AutoPopStack<ITagScript>();
		private readonly IAutoPopStack<ITemplate> templateStack = new AutoPopStack<ITemplate>();
		private UserState backofficeUser;
		private UserState frontofficeUser;
		private IAutoPopDictionaryStack<string, IPropertyBag> stack = new AutoPopDictionaryStack<string, IPropertyBag>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}