using System;
using System.Configuration;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Loads the application configuration from several configuration sources.
	/// </summary>
	public class ConfigurationSettingsApplicationInitializer : ApplicationInitializer
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService">The <see cref="IApplicationResourceService"/>.</param>
		/// <param name="tagScriptService">The <see cref="ITagScriptService"/>.</param>
		public ConfigurationSettingsApplicationInitializer(IApplicationResourceService applicationResourceService, ITagScriptService tagScriptService) : base(10)
		{
			// validate arguments
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");
			if (tagScriptService == null)
				throw new ArgumentNullException("tagScriptService");

			// set values
			this.applicationResourceService = applicationResourceService;
			this.tagScriptService = tagScriptService;
		}
		#endregion
		#region Overrides of ApplicationInitializer
		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoInitialize(IMansionContext context)
		{
			// create the application settings dataspace
			var applicationSettings = new PropertyBag
			                          {
			                          	{ApplicationSettingsConstants.IsLiveApplicationSetting, false}
			                          };

			// push the sesttings to the stack
			context.Stack.Push(ApplicationSettingsConstants.DataspaceName, applicationSettings, true).Dispose();

			// load the settings from the app.config
			LoadSettingsFromApplicationConfiguration(applicationSettings);

			// load the settings from the global settings file
			TryLoadSettingsFromGlobal(context);

			// load the settings from the environment settings file
			TryLoadSettingsFromEnvironment(context);
		}
		#endregion
		#region Load Methods
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
		private void TryLoadSettingsFromGlobal(IMansionContext context)
		{
			var globalResourcePath = new RelativeResourcePath(ResourceUtils.Combine("Global.xinclude"), false);

			// don't load settings when there is no settings file
			if (!applicationResourceService.Exists(context, globalResourcePath))
				return;

			using (var script = tagScriptService.Parse(context, applicationResourceService.GetSingle(context, globalResourcePath)))
			{
				script.Initialize(context);
				script.Execute(context);
			}
		}
		/// <summary>
		/// Tries to load settings from the environment specific settings file.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		private void TryLoadSettingsFromEnvironment(IMansionContext context)
		{
			var settingsResourcePath = new RelativeResourcePath(ResourceUtils.Combine("Settings", Environment.MachineName + ".xinclude"), false);

			// don't load settings when there is no settings file
			if (!applicationResourceService.Exists(context, settingsResourcePath))
				return;

			// load the settings
			using (var script = tagScriptService.Parse(context, applicationResourceService.GetSingle(context, settingsResourcePath)))
			{
				script.Initialize(context);
				script.Execute(context);
			}
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly ITagScriptService tagScriptService;
		#endregion
	}
}