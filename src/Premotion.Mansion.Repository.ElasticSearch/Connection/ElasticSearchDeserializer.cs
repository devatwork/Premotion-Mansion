using System.Collections.Generic;
using Newtonsoft.Json;
using Premotion.Mansion.Repository.ElasticSearch.Responses.Facets;
using RestSharp;
using RestSharp.Deserializers;

namespace Premotion.Mansion.Repository.ElasticSearch.Connection
{
	/// <summary>
	/// Default JSON deserializer for response bodies
	/// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
	/// </summary>
	public class ElasticSearchDeserializer : IDeserializer
	{
		#region Implementation of IDeserializer
		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Deserialize<T>(IRestResponse response)
		{
			return JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings
			                                                          {
			                                                          	NullValueHandling = NullValueHandling.Ignore,
			                                                          	DefaultValueHandling = DefaultValueHandling.Include,
			                                                          	Converters = new List<JsonConverter>
			                                                          	             {
			                                                          	             	new Facet.FacetReadConverter()
			                                                          	             }
			                                                          });
		}
		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string DateFormat { get; set; }
		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string RootElement { get; set; }
		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string Namespace { get; set; }
		#endregion
	}
}