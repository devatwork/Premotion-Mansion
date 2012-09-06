using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.SessionState;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Dynamo;
using Premotion.Mansion.Core.Patterns.Prioritized;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Web.Controls;
using Premotion.Mansion.Web.Controls.Forms;
using Premotion.Mansion.Web.Http;

namespace Premotion.Mansion.Web.Hosting.AspNet
{
	/// <summary>
	/// Implements the request routing for all mansion requests.
	/// </summary>
	public class MansionHttpHandlerFactory : IHttpHandlerFactory
	{
		#region Constants
		/// <summary>
		/// Identifies the query string parameter used to determine the session state type in which to serve the current request.
		/// </summary>
		public const string StateQueryStringParameterName = "session-state";
		#endregion
		#region Nested type: HttpHandlerBase
		/// <summary>
		/// Abstract implementation of the <see cref="IHttpHandler"/>.
		/// </summary>
		protected abstract class HttpHandlerBase : IHttpHandler
		{
			#region Implementation of IHttpHandler
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
			public void ProcessRequest(HttpContext context)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");

				// wrap the http context
				var wrappedContext = new HttpContextWrapper(context);

				// invoke template method
				DoProcessRequest(wrappedContext);
			}
			/// <summary>
			/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
			/// </summary>
			/// <returns>
			/// true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
			/// </returns>
			public bool IsReusable
			{
				get { return true; }
			}
			#endregion
			#region Template Methods
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="context">An <see cref="T:System.Web.HttpContextBase"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
			protected abstract void DoProcessRequest(HttpContextBase context);
			#endregion
		}
		#endregion
		#region Nested type: MansionHttpHandlerBase
		/// <summary>
		/// Base type for handlers which execute requests in a <see cref="IMansionWebContext"/>.
		/// </summary>
		protected abstract class MansionHttpHandlerBase : HttpHandlerBase
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			protected MansionHttpHandlerBase(MansionRequestHandlerBase requestHandler)
			{
				// validate arguments
				if (requestHandler == null)
					throw new ArgumentNullException("requestHandler");

				// set values
				this.requestHandler = requestHandler;
			}
			#endregion
			#region Overrides of HttpHandlerBase
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="context">An <see cref="T:System.Web.HttpContextBase"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
			protected override sealed void DoProcessRequest(HttpContextBase context)
			{
				// get the request context
				var requestContext = MansionWebContext.FetchFromHttpContext(context) as MansionWebContext;

				// initialize the the security service when there is state
				if (requestContext.HttpContext.HasSession())
					requestContext.Nucleus.ResolveSingle<ISecurityService>().InitializeSecurityContext(requestContext);

				// create the request context
				DoProcessRequest(requestContext);

				// dispose the request context
				requestContext.Dispose();
			}
			#endregion
			#region Template Methods
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="context">An <see cref="IMansionWebContext"/> object that provides references to the intrinsic mansion objects used to service HTTP requests. </param>
			protected virtual void DoProcessRequest(IMansionWebContext context)
			{
				requestHandler.Execute(context);
			}
			#endregion
			#region Private Fields
			private readonly MansionRequestHandlerBase requestHandler;
			#endregion
		}
		#endregion
		#region Nested type: ReadOnlyStateHttpHandler
		/// <summary>
		/// Implements <see cref="MansionHttpHandlerBase"/> with read-only session state. This prevents locking on the session object and allows IIS to serve several requests simultaneously for the same session.
		/// </summary>
		protected class ReadOnlyStateHttpHandler : MansionHttpHandlerBase, IReadOnlySessionState
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public ReadOnlyStateHttpHandler(MansionRequestHandlerBase requestHandler) : base(requestHandler)
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: StatefulHttpHandler
		/// <summary>
		/// Implements <see cref="MansionHttpHandlerBase"/> with session state. Requests originating from the same session can not be handled simultaneously by IIS.
		/// </summary>
		protected class StatefulHttpHandler : MansionHttpHandlerBase, IRequiresSessionState
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public StatefulHttpHandler(MansionRequestHandlerBase requestHandler) : base(requestHandler)
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: StatelesHttpHander
		/// <summary>
		/// Implements <see cref="MansionHttpHandlerBase"/> without session state. Requests originating from the same session can always be handled simultaneously by IIS.
		/// </summary>
		protected class StatelesHttpHander : MansionHttpHandlerBase
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public StatelesHttpHander(MansionRequestHandlerBase requestHandler) : base(requestHandler)
			{
			}
			#endregion
		}
		#endregion
		#region Implementation of IHttpHandlerFactory
		/// <summary>
		/// Returns an instance of a class that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Web.IHttpHandler"/> object that processes the request.
		/// </returns>
		/// <param name="context">An instance of the <see cref="T:System.Web.HttpContext"/> class that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param><param name="requestType">The HTTP data transfer method (GET or POST) that the client uses. </param><param name="url">The <see cref="P:System.Web.HttpRequest.RawUrl"/> of the requested resource. </param><param name="pathTranslated">The <see cref="P:System.Web.HttpRequest.PhysicalApplicationPath"/> to the requested resource. </param>
		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (requestType == null)
				throw new ArgumentNullException("requestType");
			if (url == null)
				throw new ArgumentNullException("url");
			if (pathTranslated == null)
				throw new ArgumentNullException("pathTranslated");

			// wrap the http context
			var wrappedContext = new HttpContextWrapper(context);

			// get the mansion request context
			var requestContext = MansionWebContext.Create(MansionWebApplicationContextFactory.Instance, wrappedContext);

			// select the handler
			var handler = HandlerList.Value.FirstOrDefault(candidate => candidate.IsSatisfiedBy(requestContext));

			// if no handler is found, it is considered an application bug
			if (handler == null)
				throw new InvalidOperationException("Could not handle request");

			// select the proper handler type
			return DoGetHandler(requestContext, handler);
		}
		/// <summary>
		/// Enables a factory to reuse an existing handler instance.
		/// </summary>
		/// <param name="handler">The <see cref="T:System.Web.IHttpHandler"/> object to reuse. </param>
		public void ReleaseHandler(IHttpHandler handler)
		{
			// do nothing
		}
		/// <summary>
		/// Returns an instance of a class that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Web.IHttpHandler"/> object that processes the request.
		/// </returns>
		/// <param name="context">An instance of the <see cref="T:System.Web.HttpContextBase"/> class that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
		/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
		protected virtual IHttpHandler DoGetHandler(IMansionWebContext context, MansionRequestHandlerBase requestHandler)
		{
			// determine if the request is stateful or not
			var requiredStateString = context.HttpContext.Request.QueryString[StateQueryStringParameterName] ?? String.Empty;

			// parse the required session state from the query srting
			var requiredByQueryString = RequiresSessionState.Parse(requiredStateString);

			// determine the highest state
			var highestStateDemanded = RequiresSessionState.DetermineHighestDemand(requiredByQueryString, requestHandler.MinimalStateDemand);

			// switch to the corrent http handler
			if (highestStateDemanded == RequiresSessionState.Full)
				return new StatefulHttpHandler(requestHandler);
			if (highestStateDemanded == RequiresSessionState.ReadOnly)
				return new ReadOnlyStateHttpHandler(requestHandler);
			if (highestStateDemanded == RequiresSessionState.No)
				return new StatelesHttpHander(requestHandler);
			throw new InvalidOperationException("Uknown session state demanded");
		}
		#endregion
		#region Private Fields
		private static readonly Lazy<IEnumerable<MansionRequestHandlerBase>> HandlerList = new Lazy<IEnumerable<MansionRequestHandlerBase>>(() =>
		                                                                                                                                    {
		                                                                                                                                    	// get the application context
		                                                                                                                                    	var applicationContext = MansionWebApplicationContextFactory.Instance;

		                                                                                                                                    	// resolve the MansionRequestHandlerBase implementations
		                                                                                                                                    	var handlers = applicationContext.Nucleus.Resolve<MansionRequestHandlerBase>();

		                                                                                                                                    	// return the sorted request handler array
		                                                                                                                                    	return handlers.OrderByPriority().ToArray();
		                                                                                                                                    });
		#endregion
	}
	/// <summary>
	/// Represents the factory for application's <see cref="IMansionContext"/>.
	/// </summary>
	public static class MansionWebApplicationContextFactory
	{
		#region Singleton Implementation
		/// <summary>
		/// Creates a single instance of the <see cref="MansionWebApplicationContextFactory"/> class.
		/// </summary>
		private static readonly Lazy<IMansionContext> InstanceFactory = new Lazy<IMansionContext>(() =>
		                                                                                          {
		                                                                                          	// make sure there is an hosted environment
		                                                                                          	if (!HostingEnvironment.IsHosted)
		                                                                                          		throw new InvalidOperationException("Premotion Mansion Web framework can only run within a hosted environment");

		                                                                                          	// create a nucleus
		                                                                                          	var nucleus = new DynamoNucleusAdapter();
		                                                                                          	nucleus.Register<IReflectionService>(t => new ReflectionService());

		                                                                                          	// create the application context
		                                                                                          	var applicationContext = new MansionContext(nucleus);

		                                                                                          	// register all the types within the assembly
		                                                                                          	nucleus.ResolveSingle<IReflectionService>().Initialize(nucleus, LoadOrderedAssemblyList());

		                                                                                          	// get all the application bootstrappers from the nucleus and allow them to bootstrap the application
		                                                                                          	foreach (var bootstrapper in nucleus.Resolve<ApplicationBootstrapper>().OrderBy(bootstrapper => bootstrapper.Weight))
		                                                                                          		bootstrapper.Bootstrap(nucleus);

		                                                                                          	// compile the nucleus for ultra fast performance
		                                                                                          	nucleus.Optimize();

		                                                                                          	// get all the application initializers from the nucleus and allow them to initialize the application
		                                                                                          	foreach (var initializer in nucleus.Resolve<ApplicationInitializer>().OrderBy(initializer => initializer.Weight))
		                                                                                          		initializer.Initialize(applicationContext);

		                                                                                          	// return the context
		                                                                                          	return applicationContext;
		                                                                                          });
		/// <summary>
		/// Gets the <see cref="IMansionContext"/>, which is the context of the entire application.
		/// </summary>
		public static IMansionContext Instance
		{
			get { return InstanceFactory.Value; }
		}
		#endregion
		#region Assembly Load Methods
		/// <summary>
		/// Loads an ordered list of assemblies.
		/// </summary>
		/// <returns>Returns the ordered list.</returns>
		private static IEnumerable<Assembly> LoadOrderedAssemblyList()
		{
			// find the directory containing the assemblies
			var binDirectory = HostingEnvironment.IsHosted ? HttpRuntime.BinDirectory : Path.GetDirectoryName(typeof (ContextFactoryHttpModule).Assembly.Location);
			if (String.IsNullOrEmpty(binDirectory))
				throw new InvalidOperationException("Could not find bin directory containing the assemblies");

			// load the assemblies
			var assemblies = Directory.GetFiles(binDirectory, "*.dll").Select(Assembly.LoadFrom);

			//  filter the assembly list include only assemblies marked with the ScanAssemblyAttribute attribute
			assemblies = assemblies.Where(candidate => candidate.IsMansionAssembly());

			// create a list of assemblies with their assembly name and their dependencies
			var assembliesWithDependecies = assemblies.Select(assembly => new
			                                                              {
			                                                              	Assembly = assembly,
			                                                              	AssemblyName = assembly.GetName(),
			                                                              	Dependencies = assembly.GetReferencedAssemblies().Where(candidate => candidate.IsMansionAssembly()).ToArray()
			                                                              });

			// keep a list of all the resolved dependencies
			var resolved = new List<string>();

			// sort the assemblies topological
			var sorted = assembliesWithDependecies.TopologicalSort(candidate =>
			                                                       {
			                                                       	// if there number of unresolved dependencies is greater than zero it is not ready to be resolved
			                                                       	if (candidate.Dependencies.Any(dependency => !resolved.Contains(dependency.Name, StringComparer.OrdinalIgnoreCase)))
			                                                       		return false;

			                                                       	// mark this assembly as resolved
			                                                       	resolved.Add(candidate.AssemblyName.Name);
			                                                       	return true;
			                                                       });

			// select only the assemblies
			return sorted.Select(x => x.Assembly);
		}
		#endregion
	}
	/// <summary>
	/// Implements the <see cref="IMansionWebContext"/>.
	/// </summary>
	public class MansionWebContext : MansionContext, IMansionWebContext
	{
		#region Constants
		/// <summary>
		/// Key under which the <see cref="IMansionWebContext"/> is stored in the <see cref="HttpContextBase.Items"/> collection.
		/// </summary>
		private const string RequestContextKey = "mansion-request-context";
		#endregion
		#region Constructors
		private MansionWebContext(IMansionContext applicationContext, HttpContextBase httpContext, IEnumerable<KeyValuePair<string, IEnumerable<IPropertyBag>>> initialStack) : base(applicationContext.Nucleus)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");
			if (initialStack == null)
				throw new ArgumentNullException("initialStack");

			// set value
			this.httpContext = httpContext;
			applicationBaseUri = new UriBuilder(httpContext.Request.Url)
			                     {
			                     	Fragment = String.Empty,
			                     	Query = String.Empty,
			                     	Path = (httpContext.Request.ApplicationPath ?? String.Empty).Trim('/') + "/"
			                     }.Uri;

			// create thet stack
			Stack = new AutoPopDictionaryStack<string, IPropertyBag>(StringComparer.OrdinalIgnoreCase, initialStack);

			// extracts needed dataspaces
			Stack.Push("Get", httpContext.Request.QueryString.ToPropertyBag(), true);
			Stack.Push("Post", httpContext.Request.Form.ToPropertyBag(), true);
			Stack.Push("Server", httpContext.Request.ServerVariables.ToPropertyBag(), true);

			// create the application dataspace
			var baseUrl = ApplicationBaseUri.ToString().TrimEnd(Dispatcher.Constants.UrlPartTrimCharacters);
			var url = httpContext.Request.Url.ToString();
			Stack.Push("Request", new PropertyBag
			                      {
			                      	{"url", url},
			                      	{"urlPath", httpContext.Request.Url.GetLeftPart(UriPartial.Path)},
			                      	{"baseUrl", baseUrl}
			                      }, true);

			// set context location flag
			IsBackoffice = url.IndexOf("/" + Constants.BackofficeUrlPrefix + "/", baseUrl.Length, StringComparison.OrdinalIgnoreCase) == baseUrl.Length;

			// initialize the context
			IPropertyBag applicationSettings;
			if (!Stack.TryPeek(ApplicationSettingsConstants.DataspaceName, out applicationSettings))
				return;

			// try to get the culture
			string defaultCulture;
			if (applicationSettings.TryGet(applicationContext, "DEFAULT_SYSTEM_CULTURE", out defaultCulture) && !string.IsNullOrEmpty(defaultCulture))
				SystemCulture = CultureInfo.GetCultureInfo(defaultCulture);

			// try to get the culture
			string defaultUICulture;
			if (applicationSettings.TryGet(applicationContext, "DEFAULT_UI_CULTURE", out defaultUICulture) && !string.IsNullOrEmpty(defaultUICulture))
				UserInterfaceCulture = CultureInfo.GetCultureInfo(defaultUICulture);

			// initialize the repository, when possible
			var repositoryNamespace = applicationSettings.Get(this, ApplicationSettingsConstants.RepositoryNamespace, string.Empty);
			if (!string.IsNullOrEmpty(repositoryNamespace))
			{
				// open the repository
				repositoryDisposable = RepositoryUtil.Open(this, repositoryNamespace, applicationSettings);
			}
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs an instance of <see cref="ContextFactoryHttpModule.MansionWebContext"/>.
		/// </summary>
		/// <param name="applicationContext">The global application context.</param>
		/// <param name="httpContext">The <see cref="HttpContextBase"/> of the current request.</param>
		/// <returns>Returns the constructed <see cref="IMansionWebContext"/>.</returns>
		public static IMansionWebContext Create(IMansionContext applicationContext, HttpContextBase httpContext)
		{
			// validate arguments
			if (applicationContext == null)
				throw new ArgumentNullException("applicationContext");
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			// create the context
			var requestContext = new MansionWebContext(applicationContext, httpContext, applicationContext.Stack);

			// store the mansion request context in the http context
			httpContext.Items[RequestContextKey] = requestContext;

			// return teh context
			return requestContext;
		}
		/// <summary>
		/// Gets the Mansion request <see cref="IMansionWebContext"/> from <paramref name="httpContext"/>.
		/// </summary>
		/// <param name="httpContext">The <see cref="HttpContextBase"/> from which to get the Mansion request context.</param>
		/// <returns>Returnss the Mansion request <see cref="IMansionWebContext"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="httpContext"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if the <see cref="IMansionWebContext"/> was not found on the <paramref name="httpContext"/>.</exception>
		public static IMansionWebContext FetchFromHttpContext(HttpContextBase httpContext)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			// get the request context
			var requestContext = httpContext.Items[RequestContextKey] as IMansionWebContext;
			if (requestContext == null)
				throw new InvalidOperationException("The request context was not found on the http context, please make sure the mansion http module is the first module");

			// return the context
			return requestContext;
		}
		#endregion
		#region Implementation of IMansionWebContext
		/// <summary>
		/// Generates a new control ID.
		/// </summary>
		/// <returns>Returns the generated control id.</returns>
		public string GetNextControlId()
		{
			return (controlIdGenerator++).ToString();
		}
		/// <summary>
		/// Tries to get the top most control.
		/// </summary>
		/// <typeparam name="TControl">The type of <see cref="Control"/> which to get.</typeparam>
		/// <param name="control">The found control.</param>
		/// <returns>Returns true when the control was found, otherwise false.</returns>
		public bool TryPeekControl<TControl>(out TControl control) where TControl : class, IControl
		{
			IControl untypedControl;
			if (!ControlStack.TryPeek(out untypedControl))
			{
				control = default(TControl);
				return false;
			}

			// try to cast the control
			control = untypedControl as TControl;
			return control != null;
		}
		/// <summary>
		/// Tries to find a control in the <see cref="ControlStack"/>.
		/// </summary>
		/// <typeparam name="TControl">The type of <see cref="Control"/> which to find.</typeparam>
		/// <param name="control">The found control.</param>
		/// <returns>Returns true when the control was found, otherwise false.</returns>
		public bool TryFindControl<TControl>(out TControl control) where TControl : class, IControl
		{
			// loop over the controls
			foreach (var candidate in ControlStack.OfType<TControl>())
			{
				control = candidate;
				return true;
			}
			control = default(TControl);
			return false;
		}
		/// <summary>
		/// Gets the top most <see cref="MailMessage"/> from the <see cref="MessageStack"/>.
		/// </summary>
		public MailMessage Message
		{
			get
			{
				// check if there is no message on the stack
				MailMessage message;
				if (!MessageStack.TryPeek(out message))
					throw new InvalidOperationException("No message was found on the stack");

				return message;
			}
		}
		/// <summary>
		/// Gets the message stack.
		/// </summary>
		public IAutoPopStack<MailMessage> MessageStack
		{
			get { return messageStack; }
		}
		/// <summary>
		/// Gets the <see cref="HttpContextBase"/>.
		/// </summary>
		public HttpContextBase HttpContext
		{
			get { return httpContext; }
		}
		/// <summary>
		/// Gets the application <see cref="Uri"/>.
		/// </summary>
		public Uri ApplicationBaseUri
		{
			get { return applicationBaseUri; }
		}
		/// <summary>
		/// Gets the <see cref="Control"/> stack.
		/// </summary>
		public IAutoPopStack<IControl> ControlStack
		{
			get { return controlStack; }
		}
		/// <summary>
		/// Gets the <see cref="Form"/> stack.
		/// </summary>
		public IAutoPopStack<Form> FormStack
		{
			get { return formStack; }
		}
		/// <summary>
		/// Get the current <see cref="Form"/> from the <see cref="FormStack"/>.
		/// </summary>
		public Form CurrentForm
		{
			get
			{
				// check if there is no form on the stack
				Form form;
				if (!FormStack.TryPeek(out form))
					throw new InvalidOperationException("No form was found on the stack");

				return form;
			}
		}
		#endregion
		#region Overrides of MansionContext
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			base.DisposeResources(disposeManagedResources);
			if (!disposeManagedResources)
				return;

			repositoryDisposable.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly Uri applicationBaseUri;
		private readonly IAutoPopStack<IControl> controlStack = new AutoPopStack<IControl>();
		private readonly IAutoPopStack<Form> formStack = new AutoPopStack<Form>();
		private readonly HttpContextBase httpContext;
		private readonly IAutoPopStack<MailMessage> messageStack = new AutoPopStack<MailMessage>();
		private readonly IDisposable repositoryDisposable;
		private int controlIdGenerator;
		#endregion
	}
}