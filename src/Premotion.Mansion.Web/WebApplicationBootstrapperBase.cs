using System;
using System.Configuration;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Web.Http;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Base class for web <see cref="ApplicationBootstrapperBase"/>.
	/// </summary>
	public abstract class WebApplicationBootstrapperBase : ApplicationBootstrapperBase
	{
		#region Overrides of ApplicationBootstrapperBase
		/// <summary>
		/// Initializes the application using the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> from which to configure the application.</param>
		protected override void DoInitialize(IConfigurableNucleus nucleus)
		{
			// allow for DI configuration
			RegisterServices(nucleus);

			// create the applicationd dataspace
			var applicationContext = ContextFactoryHttpModule.ApplicationContext;
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
		/// Initializes the application using the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> from which to configure the application.</param>
		protected abstract void RegisterServices(IConfigurableNucleus nucleus);
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Loads the settings from the app.config/web.config.
		/// </summary>
		/// <param name="applicationSettings">The <see cref="IPropertyBag"/> containing the application settings.</param>
		protected virtual void LoadSettingsFromApplicationConfiguration(IPropertyBag applicationSettings)
		{
			// get all the configuration keys
			foreach (var key in ConfigurationManager.AppSettings.AllKeys)
				applicationSettings.Set(key, ConfigurationManager.AppSettings[key]);
		}
		/// <summary>
		/// Tries to load settings from the global settings file.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected virtual void TryLoadSettingsFromGlobal(IMansionContext context)
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
		protected virtual void TryLoadSettingsFromEnvironment(IMansionContext context)
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
		protected virtual void InitializeRepository(IMansionContext context, IPropertyBag applicationSettings)
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
	}
}