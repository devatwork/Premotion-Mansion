using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses.Facets
{
	/// <summary>
	/// Represents a term <see cref="Facet"/>.
	/// </summary>
	public class TermFacet : Facet, IFacet<TermItem>
	{
		#region Overrides of Facet
		/// <summary>
		/// Maps the mapped <see cref="FacetValue"/>s.
		/// </summary>
		/// <returns>Returns the mapped <see cref="FacetValue"/>s.</returns>
		protected override IEnumerable<FacetValue> MapValues()
		{
			return Items.Select(item => new FacetValue(item.Term, item.Count));
		}
		#endregion
		#region Implementation of IFacet<out TermItem>
		/// <summary>
		/// Gets the <see cref="TermItem"/>s.
		/// </summary>
		[JsonProperty("terms")]
		public IEnumerable<TermItem> Items { get; private set; }
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of documents which have no value for the field.
		/// </summary>
		[JsonProperty("missing")]
		public int Missing { get; private set; }
		/// <summary>
		/// Gets the number of facet values not included in the returned facets.
		/// </summary>
		[JsonProperty("other")]
		public int Other { get; private set; }
		/// <summary>
		/// Gets the total number of tokens in the facet.
		/// </summary>
		[JsonProperty("total")]
		public int Total { get; private set; }
		#endregion
	}
}