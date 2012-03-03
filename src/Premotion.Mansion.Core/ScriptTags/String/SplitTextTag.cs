using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;

namespace Premotion.Mansion.Core.ScriptTags.String
{
	/// <summary>
	/// Opens a template.
	/// </summary>
	[Named(Constants.NamespaceUri, "splitText")]
	public class SplitTextTag : GetDatasetBaseTag
	{
		/// <summary>
		/// Gets the dataset.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(MansionContext context, IPropertyBag attributes)
		{
			// get the attributes
			var input = GetAttribute(context, "input", string.Empty) ?? string.Empty;

			// create a dataset
			var dataset = new Dataset();
			foreach (var part in input.Split(new[] {GetAttribute(context, "separator", ",")}, StringSplitOptions.RemoveEmptyEntries))
			{
				dataset.AddRow(new PropertyBag
				               {
				               	{"value", part}
				               });
			}
			return dataset;
		}
	}
}