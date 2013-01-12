using System.Collections.Generic;
using Newtonsoft.Json;
using Premotion.Mansion.Repository.ElasticSearch.Responses.Facets;

namespace Premotion.Mansion.Repository.ElasticSearch.Connection
{
	/// <summary>
	/// Contains the settings used for the elastic search serializer.
	/// </summary>
	public static class ElasicSearchJsonSerializerSettings
	{
		/// <summary>
		/// The <see cref="JsonSerializerSettings"/> used for elastic search.
		/// </summary>
		public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
		                                                         {
		                                                         	MissingMemberHandling = MissingMemberHandling.Ignore,
		                                                         	NullValueHandling = NullValueHandling.Ignore,
		                                                         	DefaultValueHandling = DefaultValueHandling.Include,
		                                                         	DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
		                                                         	Converters = new List<JsonConverter>
		                                                         	             {
		                                                         	             	new Facet.FacetReadConverter()
		                                                         	             }
		                                                         };
	}
}