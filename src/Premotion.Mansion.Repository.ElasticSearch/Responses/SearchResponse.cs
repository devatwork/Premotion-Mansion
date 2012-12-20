using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses
{
	/// <summary>
	/// Represents the result of a search.
	/// </summary>
	public class SearchResponse : BaseResponse
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="Hits"/>.
		/// </summary>
		[JsonProperty(PropertyName = "hits")]
		public HitMetaData Hits { get; private set; }
		/// <summary>
		/// Gets the milliseconds it took to execute the query.
		/// </summary>
		[JsonProperty(PropertyName = "took")]
		public int ElapsedMilliseconds { get; private set; }
		#endregion
	}
}