using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Creates a dataspace
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "createDataset")]
	public class CreateDatasetTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// create the dataspace and exeucte child tags
			using (context.Stack.Push(GetRequiredAttribute<string>(context, "target"), new Dataset(), GetAttribute(context, "global", false)))
				ExecuteChildTags(context);
		}
	}
}