using System;
using System.Collections.Generic;
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

			//  initialize the global data
			log.Debug("Initializing global application data");

			// get the services
			var scriptService = applicationContext.Nucleus.Get<ITagScriptService>(applicationContext);
			var resourceService = applicationContext.Nucleus.Get<IApplicationResourceService>(applicationContext);

			// check for global initialization script
			var globalResourcePath = new RelativeResourcePath(ResourceUtils.Combine("Global.xinclude"), false);
			if (resourceService.Exists(globalResourcePath))
			{
				log.Debug("Loading Global.xinclude");
				using (var script = scriptService.Parse(applicationContext, resourceService.GetSingle(applicationContext, globalResourcePath)))
				{
					script.Initialize(applicationContext);
					script.Execute(applicationContext);
				}
			}

			// check for settings file
			log.Debug("Loading environment specific settings from " + Environment.MachineName + ".xinclude");
			var settingsResourcePath = new RelativeResourcePath(ResourceUtils.Combine("Settings", Environment.MachineName + ".xinclude"), false);
			if (!resourceService.Exists(settingsResourcePath))
				throw new InvalidOperationException(string.Format("Could not find settings file {0}. Please make sure the application is configured properly", ResourceUtils.Combine("Settings", Environment.MachineName + ".xinclude")));
			using (var script = scriptService.Parse(applicationContext, resourceService.GetSingle(applicationContext, settingsResourcePath)))
			{
				script.Initialize(applicationContext);
				script.Execute(applicationContext);
			}
			log.Debug("Application started succesfully");

			// if there is a settings file check repository
			IPropertyBag applicationDataspace;
			if (applicationContext.Stack.TryPeek("Application", out applicationDataspace))
			{
				// initialize the repository, when possible
				var repositoryNamespace = applicationDataspace.Get(applicationContext, "repositoryNamespace", string.Empty);
				var repositoryConnectionString = applicationDataspace.Get(applicationContext, "repositoryConnectionString", string.Empty);
				if (!string.IsNullOrEmpty(repositoryNamespace) && !string.IsNullOrEmpty(repositoryConnectionString))
				{
					// open the repository
					using (RepositoryUtil.Open(applicationContext, repositoryNamespace, repositoryConnectionString))
					{
						// check if the root node exists
						var rootNode = applicationContext.Repository.RetrieveSingle(applicationContext, new PropertyBag
						                                                                                {
						                                                                                	{"id", 1},
						                                                                                	{"bypassAuthorization", true}
						                                                                                });
						if (!rootNode.Get(applicationContext, "repositoryInitialized", false))
						{
							var securityNode = applicationContext.Repository.Create(applicationContext, rootNode, new PropertyBag
							                                                                                      {
							                                                                                      	{"type", "SecurityIndex"},
							                                                                                      	{"name", "Security"},
							                                                                                      });
							var roleIndexNode = applicationContext.Repository.Create(applicationContext, securityNode, new PropertyBag
							                                                                                           {
							                                                                                           	{"type", "RoleIndex"},
							                                                                                           	{"name", "Roles"},
							                                                                                           });
							var adminRoleNode = applicationContext.Repository.Create(applicationContext, roleIndexNode, new PropertyBag
							                                                                                            {
							                                                                                            	{"type", "Role"},
							                                                                                            	{"name", "Administrator"},
							                                                                                            });
							var visitorRoleNode = applicationContext.Repository.Create(applicationContext, roleIndexNode, new PropertyBag
							                                                                                              {
							                                                                                              	{"type", "Role"},
							                                                                                              	{"name", "Visitor"},
							                                                                                              });
							var userIndexNode = applicationContext.Repository.Create(applicationContext, securityNode, new PropertyBag
							                                                                                           {
							                                                                                           	{"type", "UserIndex"},
							                                                                                           	{"name", "Users"},
							                                                                                           });
							var adminUserNode = applicationContext.Repository.Create(applicationContext, userIndexNode, new PropertyBag
							                                                                                            {
							                                                                                            	{"type", "User"},
							                                                                                            	{"name", "Administrator"},
							                                                                                            	{"login", "admin@premotion.nl"},
							                                                                                            	{"password", "admin"},
							                                                                                            });
							applicationContext.Repository.Create(applicationContext, userIndexNode, new PropertyBag
							                                                                        {
							                                                                        	{"type", "User"},
							                                                                        	{"name", "Visitor"},
							                                                                        	{"foreignId", "Anonymous"},
							                                                                        	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
							                                                                        });
							var userGroupIndexNode = applicationContext.Repository.Create(applicationContext, securityNode, new PropertyBag
							                                                                                                {
							                                                                                                	{"type", "UserGroupIndex"},
							                                                                                                	{"name", "Groups"},
							                                                                                                	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
							                                                                                                });
							var adminUserGroup = applicationContext.Repository.Create(applicationContext, userGroupIndexNode, new PropertyBag
							                                                                                                  {
							                                                                                                  	{"type", "UserGroup"},
							                                                                                                  	{"name", "Administrators"},
							                                                                                                  	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
							                                                                                                  	{"assignedRoleGuids", adminRoleNode.PermanentId.ToString()},
							                                                                                                  	{"userGuids", adminUserNode.PermanentId.ToString()},
							                                                                                                  });

							// update the root node
							applicationContext.Repository.Update(applicationContext, rootNode, new PropertyBag
							                                                                   {
							                                                                   	{"repositoryInitialized", true},
							                                                                   	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString() + "," + visitorRoleNode.PermanentId.ToString()},
							                                                                   });
							applicationContext.Repository.Update(applicationContext, securityNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
							applicationContext.Repository.Update(applicationContext, roleIndexNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
							applicationContext.Repository.Update(applicationContext, adminRoleNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
							applicationContext.Repository.Update(applicationContext, visitorRoleNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
							applicationContext.Repository.Update(applicationContext, userIndexNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
							applicationContext.Repository.Update(applicationContext, adminUserNode, new PropertyBag
							                                                                        {
							                                                                        	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
							                                                                        	{"foreignId", adminUserNode.PermanentId.ToString()}
							                                                                        });
							applicationContext.Repository.Update(applicationContext, adminUserGroup, new PropertyBag
							                                                                         {
							                                                                         	{"foreignId", adminUserGroup.PermanentId.ToString()}
							                                                                         });

							// add cms permission to the admin user
							applicationContext.Nucleus.Get<ISecurityModelService>(applicationContext).AssignPermission(applicationContext, new Role(adminRoleNode.PermanentId.ToString()), ProtectedOperation.Create(applicationContext, "Cms", "use"));
						}
					}
				}
			}

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