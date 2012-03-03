using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Creates a dataspace
	/// </summary>
	[Named(Constants.NamespaceUri, "createDataset")]
	public class CreateDatasetTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// create the dataspace and exeucte child tags
			using (context.Stack.Push(GetRequiredAttribute<string>(context, "target"), new Dataset(), GetAttribute(context, "global", false)))
				ExecuteChildTags(context);
		}
	}
}