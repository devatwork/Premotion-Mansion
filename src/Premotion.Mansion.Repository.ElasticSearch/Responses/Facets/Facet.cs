using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Repository.ElasticSearch.Responses.Facets
{
	/// <summary>
	/// Represents a facet.
	/// </summary>
	[JsonObject]
	public class Facet
	{
		#region Nested type: FacetReadConverter
		/// <summary>
		/// Converts <see cref="Facet"/>s.
		/// </summary>
		public class FacetReadConverter : BaseReadConverter<Facet>
		{
			#region Overrides of BaseReadConverter<Facet>
			/// <summary>
			/// Reads the JSON representation of the object.
			/// </summary>
			/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
			/// <param name="serializer">The calling serializer.</param>
			/// <returns>The object value.</returns>
			protected override object DoReadJson(JsonReader reader, JsonSerializer serializer)
			{
				var obj = JObject.Load(reader);

				JToken typeToken;
				if (obj.TryGetValue("_type", out typeToken))
				{
					var type = typeToken.Value<string>();
					switch (type)
					{
						case "terms":
							return serializer.Deserialize(obj.CreateReader(), typeof (TermFacet));
						default:
							throw new NotSupportedException(string.Format("Could not read facet of type {0}", type));
					}
				}

				// should never hit
				throw new InvalidOperationException("Detected a facet without a _type");
			}
			#endregion
		}
		#endregion
		#region Map Methods
		/// <summary>
		/// Maps this facet to a <see cref="FacetResult"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="definition">The originating <see cref="FacetDefinition"/>.</param>
		/// <returns>Returns the mapped <see cref="FacetResult"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public FacetResult Map(IMansionContext context, FacetDefinition definition)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (definition == null)
				throw new ArgumentNullException("definition");

			// map the values
			var values = MapValues();

			// creat the facet
			return FacetResult.Create(context, definition, values);
		}
		/// <summary>
		/// Maps the mapped <see cref="FacetValue"/>s.
		/// </summary>
		/// <returns>Returns the mapped <see cref="FacetValue"/>s.</returns>
		protected virtual IEnumerable<FacetValue> MapValues()
		{
			return new  FacetValue[0];
		}
		#endregion
	}
}