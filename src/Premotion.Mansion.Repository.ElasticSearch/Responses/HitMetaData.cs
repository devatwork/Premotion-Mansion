using System.Collections.Generic;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses
{
	/// <summary>
	/// Represents a collection of <see cref="Hit"/>s.
	/// </summary>
	public class HitMetaData
	{
		#region Properties
		/// <summary>
		/// Gets the total number of hits.
		/// </summary>
		[JsonProperty("total")]
		public int Total { get; private set; }
		/// <summary>
		/// Gets the maxscore of the search query.
		/// </summary>
		[JsonProperty("max_score")]
		public double MaxScore { get; private set; }
		/// <summary>
		/// Gets the <see cref="Hit"/>s returned from the search query.
		/// </summary>
		[JsonProperty("hits")]
		public List<Hit> Hits { get; private set; }
		#endregion
	}
}