using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Facets
{
	/// <summary>
	/// Base class for all facets.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class BaseFacet
	{
		#region Nested type: BaseFacetConverter
		/// <summary>
		/// Base class for base facet converters.
		/// </summary>
		/// <typeparam name="TFacet"></typeparam>
		protected abstract class BaseFacetConverter<TFacet> : BaseWriteConverter<TFacet> where TFacet : BaseFacet
		{
			#region Mappers
			/// <summary>
			/// 
			/// </summary>
			/// <param name="writer"></param>
			/// <param name="value"></param>
			/// <param name="serializer"></param>
			protected void WriteFacetFilter(JsonWriter writer, TFacet value, JsonSerializer serializer)
			{
				// if there is not filter, no not write it
				if (value.Filter == null)
					return;

				// write the facet filter
				writer.WritePropertyName("facet_filter");
				serializer.Serialize(writer, value.Filter);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Construct this facet.
		/// </summary>
		/// <param name="definition">The <see cref="FacetDefinition"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="definition"/> is null.</exception>
		protected BaseFacet(FacetDefinition definition)
		{
			// validate arguments
			if (definition == null)
				throw new ArgumentNullException("definition");

			// set the values
			Definition = definition;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="FacetDefinition"/>.
		/// </summary>
		public FacetDefinition Definition { get; private set; }
		/// <summary>
		/// Gets the <see cref="BaseFilter"/> of this facet.
		/// </summary>
		public BaseFilter Filter { get; set; }
		#endregion
	}
}