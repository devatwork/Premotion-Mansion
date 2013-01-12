using Newtonsoft.Json;
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
			return JsonConvert.DeserializeObject<T>(response.Content, ElasicSearchJsonSerializerSettings.Settings);
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