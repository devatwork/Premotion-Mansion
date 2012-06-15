using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Gets the <see cref="FacetValue"/> <see cref="Dataset"/> from a given <see cref="FacetResult"/>.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "fetchFacetValuesDataset")]
	public class FetchFacetValuesDatasetTag : GetDatasetBaseTag
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
			var facetResult = GetRequiredAttribute<FacetResult>(context, "source");

			// create the facet dataset
			var dataset = new Dataset();

			// add the facets to the dataset
			foreach (var result in facetResult.Values)
			{
				// add the row to the dataset
				dataset.AddRow(PropertyBagAdapterFactory.Adapt(context, result));
			}

			// return the dataset
			return dataset;
		}
	}
}