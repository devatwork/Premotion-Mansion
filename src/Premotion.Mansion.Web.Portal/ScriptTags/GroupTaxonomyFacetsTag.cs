using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Group the taxonomy facets for a givens <see cref="Nodeset"/>.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "groupTaxonomyFacets")]
	public class GroupTaxonomyFacetsTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// retrieve the nodeset
			var nodeset = GetRequiredAttribute<Nodeset>(context, "source");

			// get the taxonomy facet
			var facet = nodeset.Facets.FirstOrDefault(candidate => "taxonomyGuids".Equals(candidate.PropertyName, StringComparison.OrdinalIgnoreCase));
			if (facet == null)
				return;

			// remove the facet
			nodeset.RemoveFacet(facet);

			// retrieve all the taxonomy values
			var taxonomyItemsLookupTable = context.Repository.RetrieveNodeset(context, new PropertyBag
			                                                                    {
			                                                                    	{"parentPointer", "1"},
			                                                                    	{"depth", "any"},
			                                                                    	{"status", "published"},
			                                                                    	{"baseType", "TaxonomyItem"}
			                                                                    }).Nodes.ToDictionary(node => node.PermanentId, node => node);

			// combine the facet counts with the taxonomy items
			var combined = taxonomyItemsLookupTable.Join(facet.Values, entry => entry.Key.ToString(), value => value.Value.ToString(), (node, value) => new
			                                                                                                                                            {
			                                                                                                                                            	GroupName = node.Value.Pointer.Parent.Name,
			                                                                                                                                            	Value = new FacetValue(value.Value, value.Count)
			                                                                                                                                            	        {
			                                                                                                                                            	        	DisplayValue = node.Value.Pointer.Name
			                                                                                                                                            	        }
			                                                                                                                                            }, StringComparer.OrdinalIgnoreCase);

			// group the taxonomy items by their group
			var grouped = combined.GroupBy(value => value.GroupName, StringComparer.OrdinalIgnoreCase);

			// transform the grouped items into facet results
			var results = grouped.Select(group => FacetResult.Create(context, new FacetDefinition(facet.PropertyName, group.Key), group.Select(x => x.Value)));

			// add the facets
			foreach (var result in results)
				nodeset.AddFacet(result);
		}
		#endregion
	}
}