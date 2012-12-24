using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses.Facets
{
	/// <summary>
	/// Represents a term <see cref="FacetItem"/>.
	/// </summary>
	public class TermItem : FacetItem
	{
		#region Properties
		/// <summary>
		/// Gets the term.
		/// </summary>
		[JsonProperty(PropertyName = "term")]
		public string Term { get; private set; }
		#endregion
	}
}