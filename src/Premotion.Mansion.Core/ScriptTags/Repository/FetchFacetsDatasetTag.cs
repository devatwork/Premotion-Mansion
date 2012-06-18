using System.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Gets the facet <see cref="Dataset"/> from a given <see cref="Nodeset"/>.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "fetchFacetsDataset")]
	public class FetchFacetsDatasetTag : GetDatasetBaseTag
	{
		/// <summary>
		/// Gets the facet <see cref="Dataset"/> from a given <see cref="Nodeset"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(IMansionContext context, IPropertyBag attributes)
		{
			// get the nodeset
			var nodeset = GetRequiredAttribute<Nodeset>(context, "source");

			// create the facet dataset
			var dataset = new Dataset();

			// add the facets to the dataset
			foreach (var result in nodeset.Facets.OrderBy(facet => facet.FriendlyName))
			{
				// add the row to the dataset
				dataset.AddRow(PropertyBagAdapterFactory.Adapt(context, result));
			}

			// return the dataset
			return dataset;
		}
	}
}