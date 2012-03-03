using System.Collections.Generic;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Scripts
{
	/// <summary>
	/// Opens a script.
	/// </summary>
	[Named(Constants.NamespaceUri, "openScript")]
	public class OpenScriptTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the services
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var scriptService = context.Nucleus.Get<ITagScriptService>(context);

			// get the attributes
			var attributes = GetAttributes(context);
			string extension;
			if (!attributes.TryGet(context, "extension", out extension))
				attributes.Set("extension", "xinclude");

			// get the resource paths
			var resourcePath = resourceService.ParsePath(context, attributes);

			// get the resources
			IEnumerable<IResource> resources;
			if (GetAttribute(context, "checkExists", true))
				resources = resourceService.Get(context, resourcePath);
			else
				resourceService.TryGet(context, resourcePath, out resources);

			// open the templates and executes child tags
			using (scriptService.Open(context, resources))
				ExecuteChildTags(context);
		}
	}
}