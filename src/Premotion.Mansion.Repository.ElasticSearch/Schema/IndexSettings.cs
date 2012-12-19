using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Represents the index settings.
	/// </summary>
	public class IndexSettings
	{
		#region Constructors
		/// <summary>
		/// Constructs the index settings.
		/// </summary>
		public IndexSettings()
		{
			NumberOfShards = 3;
			NumberOfReplicas = 2;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets/Sets the number of shard for this index.
		/// </summary>
		/// <remarks>Default: 3</remarks>
		[JsonProperty("number_of_shards")]
		public int NumberOfShards { get; set; }
		/// <summary>
		/// Gets/Sets the number of replicas for this index.
		/// </summary>
		/// <remarks>Default: 2</remarks>
		[JsonProperty("number_of_replicas")]
		public int NumberOfReplicas { get; set; }
		#endregion
	}
}