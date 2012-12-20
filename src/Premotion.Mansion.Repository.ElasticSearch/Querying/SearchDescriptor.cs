using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Represents an ElasticSearch query.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class SearchDescriptor
	{
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
		private readonly QueryDescriptor queryDescriptor = new QueryDescriptor();
		private readonly SortDescriptor sortDescriptor = new SortDescriptor();
		#endregion
	}
}