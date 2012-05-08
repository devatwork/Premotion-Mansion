using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Dynamo;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Base type for <see cref="HttpApplication"/>s.
	/// </summary>
	public abstract class MansionHttpApplication : HttpApplication
	{
		#region Application Events
		/// <summary>
		/// Event fired when application is loaded for the first time.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Start(object sender, EventArgs e)
		{
			// make sure there is an hosted environment
			if (!HostingEnvironment.IsHosted)
				throw new InvalidOperationException("Premotion Mansion Web framework can only run within a hosted environment");

			// create a nucleus
			nucleus = new DynamoNucleusAdapter();
			nucleus.Register<IReflectionService>(t => new ReflectionService());

			// register all the types within the assembly
			nucleus.ResolveSingle<IReflectionService>().Initialize(nucleus, LoadOrderedAssemblyList());

			// get all the application bootstrappers from the nucleus
			foreach (var bootstrapper in nucleus.Resolve<ApplicationBootstrapperBase>())
				bootstrapper.Initialize(nucleus);

			// compile the nucleus for ultra fast performance
			nucleus.Optimize();

			// create the application context
			applicationContext = new MansionContext(nucleus);

			// create the applicationd dataspace
			var applicationSettings = new PropertyBag
			                          {
			                          	{ApplicationSettingsConstants.IsLiveApplicationSetting, false}
			                          };

			// push the sesttings to the stack
			applicationContext.Stack.Push(ApplicationSettingsConstants.DataspaceName, applicationSettings, true).Dispose();

			// load the configuration from web.config
			LoadSettingsFromApplicationConfiguration(applicationSettings);

			// load settings from a global settings file
			TryLoadSettingsFromGlobal(applicationContext);

			// load settings from an optional environment specific settings file
			TryLoadSettingsFromEnvironment(applicationContext);

			// initialize the repository, when possible
			InitializeRepository(applicationContext, applicationSettings);
		}
		/// <summary>
		/// Event fired when application is ended.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_End(object sender, EventArgs e)
		{
			// nothing to do yet
			applicationContext.Dispose();

			// dispose the nucleus
			nucleus.Dispose();
		}
		#endregion
		#region Configuration Methods
		/// <summary>
		/// Loads an ordered list of assemblies.
		/// </summary>
		/// <returns>Returns the ordered list.</returns>
		private static IEnumerable<Assembly> LoadOrderedAssemblyList()
		{
			// find the directory containing the assemblies
			var binDirectory = HostingEnvironment.IsHosted ? HttpRuntime.BinDirectory : Path.GetDirectoryName(typeof (MansionHttpApplication).Assembly.Location);
			if (String.IsNullOrEmpty(binDirectory))
				throw new InvalidOperationException("Could not find bin directory containing the assemblies");

			// load the assemblies
			var assemblies = Directory.GetFiles(binDirectory, "*.dll").Select(Assembly.LoadFrom);

			// order them by priority
			var orderedAssemblies = assemblies.Where(candidate => candidate.GetCustomAttributes(typeof (ScanAssemblyAttribute), false).Length > 0).OrderBy(assembly => ((ScanAssemblyAttribute) assembly.GetCustomAttributes(typeof (ScanAssemblyAttribute), false)[0]).Priority);

			// return the order list
			return orderedAssemblies;
		}
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Loads the settings from the app.config/web.config.
		/// </summary>
		/// <param name="applicationSettings">The <see cref="IPropertyBag"/> containing the application settings.</param>
		private static void LoadSettingsFromApplicationConfiguration(IPropertyBag applicationSettings)
		{
			// get all the configuration keys
			foreach (var key in ConfigurationManager.AppSettings.AllKeys)
				applicationSettings.Set(key, ConfigurationManager.AppSettings[key]);
		}
		/// <summary>
		/// Tries to load settings from the global settings file.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		private static void TryLoadSettingsFromGlobal(IMansionContext context)
		{
			var scriptService = context.Nucleus.ResolveSingle<ITagScriptService>();
			var resourceService = context.Nucleus.ResolveSingle<IApplicationResourceService>();
			var globalResourcePath = new RelativeResourcePath(ResourceUtils.Combine("Global.xinclude"), false);

			// don't load settings when there is no settings file
			if (!resourceService.Exists(context, globalResourcePath))
				return;

			using (var script = scriptService.Parse(context, resourceService.GetSingle(context, globalResourcePath)))
			{
				script.Initialize(context);
				script.Execute(context);
			}
		}
		/// <summary>
		/// Tries to load settings from the environment specific settings file.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		private static void TryLoadSettingsFromEnvironment(IMansionContext context)
		{
			var scriptService = context.Nucleus.ResolveSingle<ITagScriptService>();
			var resourceService = context.Nucleus.ResolveSingle<IApplicationResourceService>();
			var settingsResourcePath = new RelativeResourcePath(ResourceUtils.Combine("Settings", Environment.MachineName + ".xinclude"), false);

			// don't load settings when there is no settings file
			if (!resourceService.Exists(context, settingsResourcePath))
				return;

			// load the settings
			using (var script = scriptService.Parse(context, resourceService.GetSingle(context, settingsResourcePath)))
			{
				script.Initialize(context);
				script.Execute(context);
			}
		}
		/// <summary>
		/// Initializes the repository when there is one.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="applicationSettings">The <see cref="IPropertyBag"/> containing the application settings.</param>
		private static void InitializeRepository(IMansionContext context, IPropertyBag applicationSettings)
		{
			var repositoryNamespace = applicationSettings.Get(context, ApplicationSettingsConstants.RepositoryNamespace, String.Empty);

			// if not repository is specified to not try to intialize
			if (String.IsNullOrEmpty(repositoryNamespace))
				return;

			// open the repository
			using (RepositoryUtil.Open(context, repositoryNamespace, applicationSettings))
			{
				// check if the root node exists
				var rootNode = context.Repository.RetrieveSingle(context, new PropertyBag
				                                                          {
				                                                          	{"id", 1},
				                                                          	{"bypassAuthorization", true}
				                                                          });

				// if the repository is already intialized do not reinitialize it
				if (rootNode.Get(context, "repositoryInitialized", false))
					return;

				// create the default nodes
				var securityNode = context.Repository.Create(context, rootNode, new PropertyBag
				                                                                {
				                                                                	{"type", "SecurityIndex"},
				                                                                	{"name", "Security"},
				                                                                });
				var roleIndexNode = context.Repository.Create(context, securityNode, new PropertyBag
				                                                                     {
				                                                                     	{"type", "RoleIndex"},
				                                                                     	{"name", "Roles"},
				                                                                     });
				var adminRoleNode = context.Repository.Create(context, roleIndexNode, new PropertyBag
				                                                                      {
				                                                                      	{"type", "Role"},
				                                                                      	{"name", "Administrator"},
				                                                                      });
				var visitorRoleNode = context.Repository.Create(context, roleIndexNode, new PropertyBag
				                                                                        {
				                                                                        	{"type", "Role"},
				                                                                        	{"name", "Visitor"},
				                                                                        });
				var userIndexNode = context.Repository.Create(context, securityNode, new PropertyBag
				                                                                     {
				                                                                     	{"type", "UserIndex"},
				                                                                     	{"name", "Users"},
				                                                                     });
				var adminUserNode = context.Repository.Create(context, userIndexNode, new PropertyBag
				                                                                      {
				                                                                      	{"type", "User"},
				                                                                      	{"name", "Administrator"},
				                                                                      	{"login", "admin@premotion.nl"},
				                                                                      	{"password", "admin"},
				                                                                      });
				context.Repository.Create(context, userIndexNode, new PropertyBag
				                                                  {
				                                                  	{"type", "User"},
				                                                  	{"name", "Visitor"},
				                                                  	{"foreignId", "Anonymous"},
				                                                  	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                  });
				var userGroupIndexNode = context.Repository.Create(context, securityNode, new PropertyBag
				                                                                          {
				                                                                          	{"type", "UserGroupIndex"},
				                                                                          	{"name", "Groups"},
				                                                                          	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                                          });
				var adminUserGroup = context.Repository.Create(context, userGroupIndexNode, new PropertyBag
				                                                                            {
				                                                                            	{"type", "UserGroup"},
				                                                                            	{"name", "Administrators"},
				                                                                            	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                                            	{"assignedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                                            	{"userGuids", adminUserNode.PermanentId.ToString()},
				                                                                            });

				// update the root node
				context.Repository.Update(context, rootNode, new PropertyBag
				                                             {
				                                             	{"repositoryInitialized", true},
				                                             	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString() + "," + visitorRoleNode.PermanentId.ToString()},
				                                             });
				context.Repository.Update(context, securityNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.Update(context, roleIndexNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.Update(context, adminRoleNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.Update(context, visitorRoleNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.Update(context, userIndexNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.Update(context, adminUserNode, new PropertyBag
				                                                  {
				                                                  	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                  	{"foreignId", adminUserNode.PermanentId.ToString()}
				                                                  });
				context.Repository.Update(context, adminUserGroup, new PropertyBag
				                                                   {
				                                                   	{"foreignId", adminUserGroup.PermanentId.ToString()}
				                                                   });

				// add cms permission to the admin user
				context.Nucleus.ResolveSingle<ISecurityModelService>().AssignPermission(context, new Role(adminRoleNode.PermanentId.ToString()), ProtectedOperation.Create(context, "Cms", "use"));
			}
		}
		#endregion
		#region Accessor Properties
		/// <summary>
		/// Gets the <see cref="IMansionWebContext"/> of the current request.
		/// </summary>
		/// <value> </value>
		/// <exception cref="InvalidOperationException"></exception>
		public static INucleus Nucleus
		{
			get { return nucleus; }
		}
		/// <summary>
		/// Gets the <see cref="IMansionWebContext"/> of the current request.
		/// </summary>
		/// <value> </value>
		/// <exception cref="InvalidOperationException"></exception>
		public static IMansionContext ApplicationContext
		{
			get { return applicationContext; }
		}
		#endregion
		#region Private Fields
		private static MansionContext applicationContext;
		private static IConfigurableNucleus nucleus;
		#endregion
	}
}