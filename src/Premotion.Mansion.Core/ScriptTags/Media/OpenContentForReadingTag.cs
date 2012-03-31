using System;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media
{
	/// <summary>
	/// Opens a content resource for reading.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "openContentForReading")]
	public class OpenContentForReadingTag : ScriptTag
	{
		#region Constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentResourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public OpenContentForReadingTag(IContentResourceService contentResourceService)
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
			// parse the path to the resource
			var resourcePath = contentResourceService.ParsePath(context, GetAttributes(context));

			// get the resource
			var resource = contentResourceService.GetResource(context, resourcePath);

			// open the resource for reading
			using (var inputPipe = resource.OpenForReading())
			using (context.InputPipeStack.Push(inputPipe))
				ExecuteChildTags(context);
		}
		#endregion
		#region Private Fields
		private readonly IContentResourceService contentResourceService;
		#endregion
	}
}