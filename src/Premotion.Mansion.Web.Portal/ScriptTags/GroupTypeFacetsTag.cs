using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Group the type facets for a givens <see cref="Nodeset"/>.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "groupTypeFacets")]
	public class GroupTypeFacetsTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeService"></param>
		public GroupTypeFacetsTag(ITypeService typeService)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set value
			this.typeService = typeService;
		}
		#endregion
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
			var facet = nodeset.Facets.FirstOrDefault(candidate => "type".Equals(candidate.PropertyName, StringComparison.OrdinalIgnoreCase));
			if (facet == null)
				return;

			// remove the facet
			nodeset.RemoveFacet(facet);

			// get the base types on which to group
			var groupOnTypes = GetRequiredAttribute<string>(context, "baseTypes").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(typeName => typeService.Load(context, typeName));

			// group the types
			var grouped = facet.Values.Select(facetValue => new
			                                                {
			                                                	Type = typeService.Load(context, (string) facetValue.Value),
			                                                	Value = facetValue
			                                                }).GroupBy(type => groupOnTypes.FirstOrDefault(candidateParent => type.Type.IsAssignable(candidateParent)));

			// transform the grouped items into facet result
			var result = FacetResult.Create(context, new FacetDefinition(facet.PropertyName, facet.FriendlyName), grouped.Select(group => new FacetValue(group.Key, group.Aggregate(0, (current, item) => current + item.Value.Count))
			                                                                                                                              {
			                                                                                                                              	DisplayValue = group.Key != null ? group.Key.GetTypeDefinitionLabel(context) : "misc"
			                                                                                                                              }));

			// add the facet
			nodeset.AddFacet(result);
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}