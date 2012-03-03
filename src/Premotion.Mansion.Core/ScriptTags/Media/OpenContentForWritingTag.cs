using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media
{
	/// <summary>
	/// Opens a content resource for writing.
	/// </summary>
	[Named(Constants.NamespaceUri, "openContentForWriting")]
	public class OpenContentForWritingTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(MansionContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			// parse the path to the resource
			var contentResourceServive = context.Nucleus.Get<IContentResourceService>(context);
			var resourcePath = contentResourceServive.ParsePath(context, GetAttributes(context));

			// get the resource
			var resource = contentResourceServive.GetResource(context, resourcePath);

			// open the resource for writing
			using (var outputPipe = resource.OpenForWriting())
			using (context.OutputPipeStack.Push(outputPipe))
				ExecuteChildTags(context);
		}
		#endregion
	}
}