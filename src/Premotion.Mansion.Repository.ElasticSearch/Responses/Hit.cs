using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses
{
	/// <summary>
	/// Represents a single hit.
	/// </summary>
	public class Hit
	{
		#region Properties
		/// <summary>
		/// Gets the original document as JSON.
		/// </summary>
		[JsonProperty("_source")]
		public JObject Source { get; private set; }
		/// <summary>
		/// Gets the name of the index in which the result was found.
		/// </summary>
		[JsonProperty("_index")]
		public string Index { get; private set; }
		/// <summary>
		/// Gets the score of this document.
		/// </summary>
		[JsonProperty("_score")]
		public double Score { get; private set; }
		/// <summary>
		/// Gets the type of document.
		/// </summary>
		[JsonProperty("_type")]
		public string Type { get; private set; }
		/// <summary>
		/// Gets the version of the document.
		/// </summary>
		[JsonProperty("_version")]
		public string Version { get; private set; }
		/// <summary>
		/// Gets the ID of the document.
		/// </summary>
		[JsonProperty("_id")]
		public string Id { get; private set; }
		#endregion
	}
}