using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Maps the source store behavior.
	/// </summary>
	public class TypeMappingSource
	{
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="TypeMappingSource"/>.
		/// </summary>
		public TypeMappingSource()
		{
			Enabled = true;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets/Sets a flag indicating whether the actual JSON that was used as the indexed document is stored or not.
		/// </summary>
		/// <returns>Defaults to true.</returns>
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }
		#endregion
	}
}