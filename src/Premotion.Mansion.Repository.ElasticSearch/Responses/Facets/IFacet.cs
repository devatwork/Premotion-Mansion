using System.Collections.Generic;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses.Facets
{
	/// <summary>
	/// Represents a facet.
	/// </summary>
	/// <typeparam name="TFacetItem">The type of <see cref="FacetItem"/>.</typeparam>
	[JsonObject]
	public interface IFacet<out TFacetItem> where TFacetItem : FacetItem
	{
		#region Properties
		/// <summary>
		/// Gets the <typeparamref name="TFacetItem"/>s.
		/// </summary>
		IEnumerable<TFacetItem> Items { get; }
		#endregion
	}
}