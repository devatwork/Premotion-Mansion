#region License
//   Copyright 2010 John Sheehan
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion
#region Acknowledgements
// Original JsonSerializer contributed by Daniel Crenna (@dimebrain)
#endregion
using System.IO;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace Premotion.Mansion.Repository.ElasticSearch.Connection
{
	/// <summary>
	/// Default JSON serializer for request bodies
	/// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
	/// </summary>
	public class ElasticSearchJsonSerializer : ISerializer
	{
		private readonly Newtonsoft.Json.JsonSerializer serializer;

		/// <summary>
		/// Default serializer
		/// </summary>
		public ElasticSearchJsonSerializer()
		{
			ContentType = "application/json";
			serializer = new Newtonsoft.Json.JsonSerializer
			             {
			             	MissingMemberHandling = MissingMemberHandling.Ignore,
			             	NullValueHandling = NullValueHandling.Ignore,
			             	DefaultValueHandling = DefaultValueHandling.Include
			             };
		}

		/// <summary>
		/// Default serializer with overload for allowing custom Json.NET settings
		/// </summary>
		public ElasticSearchJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
		{
			ContentType = "application/json";
			this.serializer = serializer;
		}
		#region ISerializer Members
		/// <summary>
		/// Serialize the object as JSON
		/// </summary>
		/// <param name="obj">Object to serialize</param>
		/// <returns>JSON as String</returns>
		public string Serialize(object obj)
		{
			using (var stringWriter = new StringWriter())
			using (var jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				jsonTextWriter.Formatting = Formatting.Indented;
				jsonTextWriter.QuoteChar = '"';

				serializer.Serialize(jsonTextWriter, obj);

				var result = stringWriter.ToString();
				return result;
			}
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
		/// <summary>
		/// Content type for serialized content
		/// </summary>
		public string ContentType { get; set; }
		#endregion
	}
}