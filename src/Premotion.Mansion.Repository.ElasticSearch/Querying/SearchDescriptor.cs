using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Represents an ElasticSearch query.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class SearchDescriptor
	{
		#region Constructors
		/// <summary>
		/// Constructs a new search descriptor.
		/// </summary>
		/// <param name="indexDefinition">The <see cref="IndexDefinition"/> on which this search will be executed.</param>
		/// <param name="typeMapping">The <see cref="TypeMapping"/> on which this search will be executed.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public SearchDescriptor(IndexDefinition indexDefinition, TypeMapping typeMapping)
		{
			// validate arguments
			if (indexDefinition == null)
				throw new ArgumentNullException("indexDefinition");
			if (typeMapping == null)
				throw new ArgumentNullException("typeMapping");

			// set the values
			this.indexDefinition = indexDefinition;
			this.typeMapping = typeMapping;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="QueryDescriptor"/>.
		/// </summary>
		public QueryDescriptor Queries
		{
			get { return queryDescriptor; }
		}
		/// <summary>
		/// Gets the <see cref="SortDescriptor"/>.
		/// </summary>
		public SortDescriptor Sorts
		{
			get { return sortDescriptor; }
		}
		/// <summary>
		/// Gets the <see cref="FilterDescriptor"/>.
		/// </summary>
		public FilterDescriptor Filters
		{
			get { return filterDescriptor; }
		}
		/// <summary>
		/// Gets the <see cref="FacetDescriptor"/>.
		/// </summary>
		public FacetDescriptor Facets
		{
			get { return facetDescriptor; }
		}
		/// <summary>
		/// Gets the <see cref="IndexDefinition"/> on which this search will be executed.
		/// </summary>
		public IndexDefinition IndexDefinition
		{
			get { return indexDefinition; }
		}
		/// <summary>
		/// Gets the <see cref="TypeMapping"/> on which this search will be executed.
		/// </summary>
		public TypeMapping TypeMapping
		{
			get { return typeMapping; }
		}
		#endregion
		#region Json Mapping Properties
		[JsonProperty("query")]
		private QueryDescriptor MappedQueryDescriptor
		{
			get { throw new NotImplementedException(); }
		}
		[JsonProperty("sort")]
		private IDictionary<string, object> MappedSortDescriptor
		{
			get { throw new NotImplementedException(); }
		}
		[JsonProperty("query")]
		private FilterDescriptor MappedFilterDescriptor
		{
			get { throw new NotImplementedException(); }
		}
		[JsonProperty("facets")]
		private FacetDescriptor MappedFacetDescriptor
		{
			get { throw new NotImplementedException(); }
		}
		#endregion
		#region Private Fields
		private readonly FacetDescriptor facetDescriptor = new FacetDescriptor();
		private readonly FilterDescriptor filterDescriptor = new FilterDescriptor();
		private readonly IndexDefinition indexDefinition;
		private readonly QueryDescriptor queryDescriptor = new QueryDescriptor();
		private readonly SortDescriptor sortDescriptor = new SortDescriptor();
		private readonly TypeMapping typeMapping;
		#endregion
	}
}