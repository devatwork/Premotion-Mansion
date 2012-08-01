using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Dynamo;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Web.Controls;
using Premotion.Mansion.Web.Controls.Forms;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements <see cref="IHttpModule"/> for mansion web application.
	/// </summary>
	public class ContextFactoryHttpModule : DisposableBase, IHttpModule
	{
		#region Static Constructors
		private static readonly IConfigurableNucleus nucleus;
		private static readonly MansionContext applicationContext;
		/// <summary>
		/// Initializes the application.
		/// </summary>
		static ContextFactoryHttpModule()
		{
			// make sure there is an hosted environment
			if (!HostingEnvironment.IsHosted)
				throw new InvalidOperationException("Premotion Mansion Web framework can only run within a hosted environment");

			// create a nucleus
			nucleus = new DynamoNucleusAdapter();
			nucleus.Register<IReflectionService>(t => new ReflectionService());

			// create the application context
			applicationContext = new MansionContext(nucleus);

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
		}
		/// <summary>
		/// Gets the <see cref="IMansionWebContext"/> of the current request.
		/// </summary>
		/// <value> </value>
		/// <exception cref="InvalidOperationException"></exception>
		public static INucleus Nucleus
		{
			get
			{
				// guard
				if (nucleus == null)
					throw new InvalidOperationException("Could not get the nucleus, make sure the ContextFactoryHttpModule is registered");

				return nucleus;
			}
		}
		/// <summary>
		/// Gets the <see cref="IMansionWebContext"/> of the current request.
		/// </summary>
		/// <value> </value>
		/// <exception cref="InvalidOperationException"></exception>
		public static IMansionContext ApplicationContext
		{
			get
			{
				// guard
				if (applicationContext == null)
					throw new InvalidOperationException("Could not get the applicationContext, make sure the ContextFactoryHttpModule is registered");

				return applicationContext;
			}
		}
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
		#region Constants
		/// <summary>
		/// Key under which the <see cref="IMansionWebContext"/> is stored in the <see cref="HttpContextBase.Items"/> collection.
		/// </summary>
		private const string RequestContextKey = "mansion-request-context";
		#endregion
		#region Nested type: MansionWebContext
		/// <summary>
		/// Implements the <see cref="IMansionWebContext"/>.
		/// </summary>
		private class MansionWebContext : MansionContext, IMansionWebContext
		{
			#region Constructors
			private MansionWebContext(INucleus nucleus, HttpContextBase httpContext, IEnumerable<KeyValuePair<string, IEnumerable<IPropertyBag>>> initialStack) : base(nucleus)
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
				if (applicationSettings.TryGet(applicationContext, "DEFAULT_UI_CULTURE", out defaultCulture) && !string.IsNullOrEmpty(defaultCulture))
					UserInterfaceCulture = CultureInfo.GetCultureInfo(defaultCulture);

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
			/// Constructs an instance of <see cref="MansionWebContext"/>.
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
				return new MansionWebContext(applicationContext.Nucleus, httpContext, applicationContext.Stack);
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
		#endregion
		#region Implementation of IHttpModule
		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
		public void Init(HttpApplication context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// hook into the request
			context.BeginRequest += (sender, args) =>
			                        {
			                        	// wrap the http context
			                        	var httpContext = new HttpContextWrapper(HttpContext.Current);

			                        	// initialize the mansion web context
			                        	var requestContext = MansionWebContext.Create(ApplicationContext, httpContext);

			                        	// store it in the request
			                        	httpContext.Items[RequestContextKey] = requestContext;
			                        };
			context.PostAcquireRequestState += (sender, args) =>
			                                   {
			                                   	// get the mansion web request
			                                   	var requestContext = RequestContext as MansionWebContext;
			                                   	if (requestContext == null)
			                                   		throw new InvalidOperationException("The request context was not found on the http context, please make sure the mansion http module is the first module");

			                                   	// initialize the security context
			                                   	requestContext.Nucleus.ResolveSingle<ISecurityService>().InitializeSecurityContext(requestContext);
			                                   };
			context.EndRequest += (sender, args) =>
			                      {
			                      	// get the mansion web request
			                      	var requestContext = RequestContext as MansionWebContext;
			                      	if (requestContext == null)
			                      		throw new InvalidOperationException("The request context was not found on the http context, please make sure the mansion http module is the first module");

			                      	// dispose the context properly
			                      	requestContext.Dispose();
			                      };
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Gets the <see cref="IMansionWebContext"/> of the current request.
		/// </summary>
		/// <value> </value>
		/// <exception cref="InvalidOperationException"></exception>
		public static IMansionWebContext RequestContext
		{
			get
			{
				// get the request context
				var requestContext = HttpContext.Current.Items[RequestContextKey] as IMansionWebContext;
				if (requestContext == null)
					throw new InvalidOperationException("The request context was not found on the http context, please make sure the mansion http module is the first module");

				// return the context
				return requestContext;
			}
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
		}
		#endregion
	}
}