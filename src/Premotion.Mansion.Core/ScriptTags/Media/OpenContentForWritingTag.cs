using System;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media
{
	/// <summary>
	/// Opens a content resource for writing.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "openContentForWriting")]
	public class OpenContentForWritingTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentResourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public OpenContentForWritingTag(IContentResourceService contentResourceService)
		{
			// validate arguments
			if (contentResourceService == null)
				throw new ArgumentNullException("contentResourceService");

			// set values
			this.contentResourceService = contentResourceService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			// parse the path to the resource
			var resourcePath = contentResourceService.ParsePath(context, GetAttributes(context));

			// get the resource
			var resource = contentResourceService.GetResource(context, resourcePath);

			// open the resource for writing
			using (var outputPipe = resource.OpenForWriting())
			using (context.OutputPipeStack.Push(outputPipe))
				ExecuteChildTags(context);
		}
		#endregion
		#region Private Fields
		private readonly IContentResourceService contentResourceService;
		#endregion
	}
}