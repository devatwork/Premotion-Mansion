using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Implements a <see cref="Record"/> set.
	/// </summary>
	public class RecordSet : Dataset
	{
		#region Constructors
		/// <summary>
		/// Creates a filled nodeset.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties"></param>
		/// <param name="records"></param>
		public RecordSet(IMansionContext context, IPropertyBag properties, IEnumerable<Record> records) : base(properties)
		{
			// validate arguments
			if (records == null)
				throw new ArgumentNullException("records");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// set values
			foreach (var node in records)
				RowCollection.Add(node);
			Set("count", RowCollection.Count);

			// check for paging
			var totalRowCount = properties.Get(context, "totalCount", -1);
			var pageNumber = properties.Get(context, "pageNumber", -1);
			var rowsPerPage = properties.Get(context, "pageSize", -1);
			if (totalRowCount != -1 && pageNumber != -1 && rowsPerPage != -1)
				SetPaging(totalRowCount, pageNumber, rowsPerPage);

			// check for sort
			string sortString;
			if (properties.TryGet(context, "sort", out sortString))
			{
				foreach (var sort in Collections.Sort.Parse(sortString))
					AddSort(sort);
			}
		}
		#endregion
		#region Row Methods
		/// <summary>
		/// Adds a new row to this set.
		/// </summary>
		/// <param name="row">The row which to add.</param>
		public override void AddRow(IPropertyBag row)
		{
			// validate arguments
			if (row == null)
				throw new ArgumentNullException("row");

			// make sure the row is a node
			if (! (row is Record))
				throw new InvalidOperationException("The row must be a record.");

			RowCollection.Add(row);
			Set("count", RowCollection.Count);
		}
		/// <summary>
		/// Removes a row from this set.
		/// </summary>
		/// <param name="row">The row which to remove.</param>
		public override void RemoveRow(IPropertyBag row)
		{
			// validate arguments
			if (row == null)
				throw new ArgumentNullException("row");

			// make sure the row is a node
			if (! (row is Record))
				throw new InvalidOperationException("The row must be a record.");

			RowCollection.Remove(row);
			Set("count", RowCollection.Count);
		}
		#endregion
		#region Facet Methods
		/// <summary>
		/// Adds an <see cref="FacetResult"/> to this nodeset.
		/// </summary>
		/// <param name="result">The <see cref="FacetResult"/> which to add.</param>
		public void AddFacet(FacetResult result)
		{
			// validate arguments
			if (result == null)
				throw new ArgumentNullException("result");

			// add the result
			facetResults.Add(result);
		}
		/// <summary>
		/// Removes the given <paramref name="facet"/> from the <see cref="Facet"/>s.
		/// </summary>
		/// <param name="facet">The <see cref="FacetResult"/> which to remove.</param>
		public void RemoveFacet(FacetResult facet)
		{
			// validate arguments
			if (facet == null)
				throw new ArgumentNullException("facet");

			// remove the facet
			facetResults.Remove(facet);
		}
		#endregion
		#region Facet Properties
		/// <summary>
		/// Gets the <see cref="FacetResult"/>s.
		/// </summary>
		public IEnumerable<FacetResult> Facets
		{
			get { return facetResults; }
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the nodes in this set.
		/// </summary>
		public IEnumerable<Record> Records
		{
			get { return RowCollection.Select(x => (Node) x); }
		}
		#endregion
		#region Private Fields
		private readonly ICollection<FacetResult> facetResults = new List<FacetResult>();
		#endregion
	}
}