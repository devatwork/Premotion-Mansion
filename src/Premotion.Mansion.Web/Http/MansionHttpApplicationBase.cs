using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;
using log4net;
using log4net.Config;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Base class for all mansion <see cref="HttpApplication"/>s.
	/// </summary>
	public abstract class MansionHttpApplicationBase : HttpApplication
	{
		#region Application Events
		/// <summary>
		/// Event fired when application is loaded for the first time.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Start(object sender, EventArgs e)
		{
			// start logging
			XmlConfigurator.Configure();
			Error += (obj, ea) =>
			         {
			         	// get the exception
			         	var exception = Server.GetLastError();

			         	// log it
			         	log.Error("Unhandled exception left the page", exception);
			         };
			log.Info("Starting application with error logging");

			// create the context
			log.Debug("Creating the application context");
			var applicationContext = InitializeApplicationContext();

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

			// store the application context
			InternalApplicationContext = applicationContext;
		}
		/// <summary>
		/// Event fired when application is ended.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_End(object sender, EventArgs e)
		{
			// clean up the application
			log.Info("Stopping application");
			InternalApplicationContext.Dispose();
			InternalApplicationContext = null;
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Initializes the <see cref="MansionContext"/> used by this application.
		/// </summary>
		/// <returns>Return the initialized <see cref="MansionContext"/>.</returns>
		protected abstract MansionApplicationContext InitializeApplicationContext();
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
		private static void TryLoadSettingsFromGlobal(MansionContext context)
		{
			log.Debug("Loading settings from Global.xinclude");
			var scriptService = context.Nucleus.Get<ITagScriptService>(context);
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var globalResourcePath = new RelativeResourcePath(ResourceUtils.Combine("Global.xinclude"), false);

			// don't load settings when there is no settings file
			if (!resourceService.Exists(globalResourcePath))
			{
				log.Debug(" - No settings loaded from Global.xinclude because the file does not exist");
				return;
			}

			using (var script = scriptService.Parse(context, resourceService.GetSingle(context, globalResourcePath)))
			{
				script.Initialize(context);
				script.Execute(context);
			}
			log.Debug(" - Loaded settings from Global.xinclude");
		}
		/// <summary>
		/// Tries to load settings from the environment specific settings file.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		private static void TryLoadSettingsFromEnvironment(MansionContext context)
		{
			log.Debug("Loading environment specific settings from " + Environment.MachineName + ".xinclude");
			var scriptService = context.Nucleus.Get<ITagScriptService>(context);
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var settingsResourcePath = new RelativeResourcePath(ResourceUtils.Combine("Settings", Environment.MachineName + ".xinclude"), false);

			// don't load settings when there is no settings file
			if (!resourceService.Exists(settingsResourcePath))
			{
				log.Debug("- No settings loaded from " + Environment.MachineName + ".xinclude because the file does not exist");
				return;
			}

			// load the settings
			using (var script = scriptService.Parse(context, resourceService.GetSingle(context, settingsResourcePath)))
			{
				script.Initialize(context);
				script.Execute(context);
			}
			log.Debug(" - Loaded settings from " + Environment.MachineName + ".xinclude");
		}
		/// <summary>
		/// Initializes the repository when there is one.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="applicationSettings">The <see cref="IPropertyBag"/> containing the application settings.</param>
		private static void InitializeRepository(MansionContext context, IPropertyBag applicationSettings)
		{
			log.Debug("Initializing repository");
			var repositoryNamespace = applicationSettings.Get(context, ApplicationSettingsConstants.RepositoryNamespace, string.Empty);

			// if not repository is specified to not try to intialize
			if (string.IsNullOrEmpty(repositoryNamespace))
			{
				log.Debug(" - No repository found to initialize");
				return;
			}

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
				{
					log.Debug(" - Repository already intialized");
					return;
				}

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
				context.Nucleus.Get<ISecurityModelService>(context).AssignPermission(context, new Role(adminRoleNode.PermanentId.ToString()), ProtectedOperation.Create(context, "Cms", "use"));
			}

			log.Debug(" - Finished initializing the repository");
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="MansionHttpApplicationBase"/> instance of this application.
		/// </summary>
		private static MansionHttpApplicationBase Current
		{
			get
			{
				var current = HttpContext.Current.ApplicationInstance as MansionHttpApplicationBase;
				if (current == null)
					throw new InvalidOperationException("The current application does not inherit from MansionHttpApplicationBase, please check Global.asax");
				return current;
			}
		}
		/// <summary>
		/// Gets the <see cref="MansionContext"/>.
		/// </summary>
		private MansionApplicationContext InternalApplicationContext
		{
			get
			{
				var context = Application["context"] as MansionApplicationContext;
				if (context == null)
					throw new InvalidOperationException("Could not find application context within the global application data. Please check Global.asax");
				return context;
			}
			set { Application["context"] = value; }
		}
		/// <summary>
		/// Gets the initialized web application context.
		/// </summary>
		public static INucleusAwareContext ApplicationContext
		{
			get { return Current.InternalApplicationContext; }
		}
		/// <summary>
		/// Gets the global data of this application.
		/// </summary>
		public static IEnumerable<KeyValuePair<string, IEnumerable<IPropertyBag>>> GlobalData
		{
			get { return Current.InternalApplicationContext.Stack; }
		}
		#endregion
		#region Private Fields
		private static readonly ILog log = LogManager.GetLogger(typeof (MansionHttpApplicationBase));
		#endregion
	}
}