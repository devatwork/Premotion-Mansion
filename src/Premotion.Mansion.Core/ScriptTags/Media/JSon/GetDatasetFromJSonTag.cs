using System;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.JSon
{
	/// <summary>
	/// Gets an <see cref="Dataset"/> from a JSon object.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "getDatasetFromJSon")]
	public class GetDatasetFromJSonTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the source
			var input = GetAttribute<string>(context, "input");

			// if there is content to parse, parse it
			Dataset dataset = null;
			if (!string.IsNullOrEmpty(input))
			{
				// parse the dataset
				dataset = JArray.Parse(input).ToDataset();
			}

			// check if there is a source
			if (dataset != null)
			{
				// create the dataspace and exeucte child tags
				using (context.Stack.Push(GetRequiredAttribute<string>(context, "target"), dataset, GetAttribute(context, "global", false)))
					ExecuteChildTags(context);
			}
			else
			{
				// check for alternative branch
				NotFoundTag notFound;
				if (!TryGetAlternativeChildTag(out notFound))
					throw new InvalidOperationException("Could not parse ");
				notFound.Execute(context);
			}
		}
		#endregion
	}
}