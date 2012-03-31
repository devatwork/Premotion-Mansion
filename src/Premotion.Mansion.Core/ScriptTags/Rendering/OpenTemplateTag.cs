using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Core.ScriptTags.Rendering
{
	/// <summary>
	/// Opens a template.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "openTemplate")]
	public class OpenTemplateTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <param name="templateService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public OpenTemplateTag(IApplicationResourceService applicationResourceService, ITemplateService templateService)
		{
			// validate arguments
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");
			if (templateService == null)
				throw new ArgumentNullException("templateService");

			//set values
			this.applicationResourceService = applicationResourceService;
			this.templateService = templateService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the attributes
			var attributes = GetAttributes(context);
			string extension;
			if (!attributes.TryGet(context, "extension", out extension))
				attributes.Set("extension", TemplateServiceConstants.DefaultTemplateExtension);

			// get the resource paths
			var resourcePath = applicationResourceService.ParsePath(context, attributes);

			// get the resources
			IEnumerable<IResource> resources;
			if (GetAttribute(context, "checkExists", true))
				resources = applicationResourceService.Get(context, resourcePath);
			else
				applicationResourceService.TryGet(context, resourcePath, out resources);

			// open the templates and executes child tags);
			using (templateService.Open(context, resources))
				ExecuteChildTags(context);
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly ITemplateService templateService;
		#endregion
	}
}