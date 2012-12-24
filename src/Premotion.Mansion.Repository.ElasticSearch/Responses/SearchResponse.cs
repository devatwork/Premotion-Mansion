using System.Collections.Generic;
using Newtonsoft.Json;
using Premotion.Mansion.Repository.ElasticSearch.Responses.Facets;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses
{
	/// <summary>
	/// Represents the result of a search.
	/// </summary>
	[JsonObject]
	public class SearchResponse : BaseResponse
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="Hits"/>.
		/// </summary>
		[JsonProperty("hits")]
		public HitMetaData Hits { get; private set; }
		/// <summary>
		/// Gets the <see cref="Facet"/>s.
		/// </summary>
		[JsonProperty("facets")]
		public IDictionary<string, Facet> Facets { get; set; }
		/// <summary>
		/// Gets the milliseconds it took to execute the query.
		/// </summary>
		[JsonProperty(PropertyName = "took")]
		public int ElapsedMilliseconds { get; private set; }
		#endregion
	}
}