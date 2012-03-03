using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Core.ScriptTags.Rendering
{
	/// <summary>
	/// Opens a template.
	/// </summary>
	[Named(Constants.NamespaceUri, "openTemplate")]
	public class OpenTemplateTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the services
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var templateService = context.Nucleus.Get<ITemplateService>(context);

			// get the attributes
			var attributes = GetAttributes(context);
			string extension;
			if (!attributes.TryGet(context, "extension", out extension))
				attributes.Set("extension", TemplateServiceConstants.DefaultTemplateExtension);

			// get the resource paths
			var resourcePath = resourceService.ParsePath(context, attributes);

			// get the resources
			IEnumerable<IResource> resources;
			if (GetAttribute(context, "checkExists", true))
				resources = resourceService.Get(context, resourcePath);
			else
				resourceService.TryGet(context, resourcePath, out resources);

			// open the templates and executes child tags);
			using (templateService.Open(context, resources))
				ExecuteChildTags(context);
		}
	}
}