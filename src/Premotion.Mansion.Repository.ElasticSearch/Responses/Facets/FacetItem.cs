using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses.Facets
{
	/// <summary>
	/// Represents a facet item.
	/// </summary>
	public abstract class FacetItem
	{
		#region Properties
		/// <summary>
		/// Gets the count of this item.
		/// </summary>
		[JsonProperty("count")]
		public virtual int Count { get; private set; }
		#endregion
	}
}