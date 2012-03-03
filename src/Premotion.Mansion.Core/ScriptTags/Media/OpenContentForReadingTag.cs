using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media
{
	/// <summary>
	/// Opens a content resource for reading.
	/// </summary>
	[Named(Constants.NamespaceUri, "openContentForReading")]
	public class OpenContentForReadingTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(MansionContext context)
		{
			// parse the path to the resource
			var contentResourceServive = context.Nucleus.Get<IContentResourceService>(context);
			var resourcePath = contentResourceServive.ParsePath(context, GetAttributes(context));

			// get the resource
			var resource = contentResourceServive.GetResource(context, resourcePath);

			// open the resource for reading
			using (var inputPipe = resource.OpenForReading())
			using (context.InputPipeStack.Push(inputPipe))
				ExecuteChildTags(context);
		}
		#endregion
	}
}