using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Fetches a dataspace from the stack.
	/// </summary>
	[Named(Constants.NamespaceUri, "fetchDataspace")]
	public class FetchDataspaceTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the attributes
			var source = GetAttribute<IPropertyBag>(context, "source");

			// check if there is a source
			if (source != null)
			{
				// create the dataspace and exeucte child tags
				using (context.Stack.Push(GetRequiredAttribute<string>(context, "target"), source, GetAttribute(context, "global", false)))
					ExecuteChildTags(context);
			}
			else
			{
				// check for alternative branch
				NotFoundTag notFound;
				if (!TryGetAlternativeChildTag(out notFound))
					throw new InvalidOperationException("Dataspace not found and no notFound tag detected");
				notFound.Execute(context);
			}
		}
	}
}