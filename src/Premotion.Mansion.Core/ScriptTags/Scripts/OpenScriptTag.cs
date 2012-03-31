using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Scripts
{
	/// <summary>
	/// Opens a script.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "openScript")]
	public class OpenScriptTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <param name="tagScriptService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public OpenScriptTag(IApplicationResourceService applicationResourceService, ITagScriptService tagScriptService)
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
		#region Overrides of ScriptTag
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the attributes
			var attributes = GetAttributes(context);
			string extension;
			if (!attributes.TryGet(context, "extension", out extension))
				attributes.Set("extension", "xinclude");

			// get the resource paths
			var resourcePath = applicationResourceService.ParsePath(context, attributes);

			// get the resources
			IEnumerable<IResource> resources;
			if (GetAttribute(context, "checkExists", true))
				resources = applicationResourceService.Get(context, resourcePath);
			else
				applicationResourceService.TryGet(context, resourcePath, out resources);

			// open the templates and executes child tags
			using (tagScriptService.Open(context, resources))
				ExecuteChildTags(context);
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly ITagScriptService tagScriptService;
		#endregion
	}
}