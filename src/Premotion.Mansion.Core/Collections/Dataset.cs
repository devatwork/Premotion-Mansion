using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Represets a set of <see cref="IPropertyBag"/>.
	/// </summary>
	public class Dataset : PropertyBag
	{
		#region Constructors
		/// <summary>
		/// Constructs a dataset.
		/// </summary>
		public Dataset()
		{
			IsPaged = false;
		}
		/// <summary>
		/// Constructs a dataset.
		/// </summary>
		/// <param name="properties">The initial properties of this dataset.</param>
		protected Dataset(IEnumerable<KeyValuePair<string, object>> properties) : base(properties)
		{
			IsPaged = false;
		}
		/// <summary>
		/// Constructs a <see cref="Dataset"/> with the given <paramref name="properties"/> and <paramref name="rows"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties of this set.</param>
		/// <param name="rows">The list of rows.</param>
		public Dataset(IMansionContext context, IPropertyBag properties, IEnumerable<IPropertyBag> rows) : base(properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (rows == null)
				throw new ArgumentNullException("rows");

			// set values
			foreach (var row in rows)
				RowCollection.Add(row);
			Set("count", RowCollection.Count);

			// check for paging
			var totalRowCount = properties.Get(context, "totalCount", -1);
			var pageNumber = properties.Get(context, "pageNumber", -1);
			var rowsPerPage = properties.Get(context, "pageSize", -1);
			if (totalRowCount != -1 && pageNumber != -1 && rowsPerPage != -1)
				SetPaging(totalRowCount, pageNumber, rowsPerPage);
			else
				IsPaged = false;

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
		public virtual void AddRow(IPropertyBag row)
		{
			// validate arguments
			if (row == null)
				throw new ArgumentNullException("row");

			RowCollection.Add(row);
			Set("count", RowCollection.Count);
		}
		/// <summary>
		/// Removes a row from this set.
		/// </summary>
		/// <param name="row">The row which to remove.</param>
		public virtual void RemoveRow(IPropertyBag row)
		{
			// validate arguments
			if (row == null)
				throw new ArgumentNullException("row");

			RowCollection.Remove(row);
			Set("count", RowCollection.Count);
		}
		/// <summary>
		/// Gets the row at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>Returns the row.</returns>
		public IPropertyBag GetRow(int index)
		{
			return RowCollection[index];
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets all the rows in this dataset. Each row is represented as a <see cref="IPropertyBag"/>.
		/// </summary>
		public IEnumerable<IPropertyBag> Rows
		{
			get { return rows; }
		}
		/// <summary>
		/// Gets the number of rows in this dataset.
		/// </summary>
		public int RowCount
		{
			get { return RowCollection.Count; }
		}
		/// <summary>
		/// Gets the row collection of this set.
		/// </summary>
		protected IList<IPropertyBag> RowCollection
		{
			get { return rows; }
		}
		#endregion
		#region Sorting Methods
		/// <summary>
		/// Sorts this dataset using the provided <paramref name="comparison"/>.
		/// </summary>
		/// <param name="comparison">The <see cref="Comparison{T}"/> which to use.</param>
		public void Sort(Comparison<IPropertyBag> comparison)
		{
			// validate arguments
			if (comparison == null)
				throw new ArgumentNullException("comparison");

			rows.Sort(comparison);
		}
		/// <summary>
		/// Adds <paramref name="sort"/> to the sorts of this dataset.
		/// </summary>
		/// <param name="sort">The <see cref="Sort"/> which to add.</param>
		protected void AddSort(Sort sort)
		{
			// validate arguments
			if (sort == null)
				throw new ArgumentNullException("sort");

			// add it to the list
			sorts.Add(sort);
		}
		#endregion
		#region Sorting Properties
		/// <summary>
		/// Gets the <see cref="Sort"/>s of this dataset.
		/// </summary>
		public IEnumerable<Sort> Sorts
		{
			get { return sorts; }
		}
		#endregion
		#region Paging Methods
		/// <summary>
		/// Sets paging for this dataset.
		/// </summary>
		/// <param name="totalCount">The total number of rows in the complete set.</param>
		/// <param name="pageNumber">The current page this dataset displays.</param>
		/// <param name="rowsPerPage">The number of rows per page.</param>
		protected void SetPaging(int totalCount, int pageNumber, int rowsPerPage)
		{
			IsPaged = true;
			currentPage = pageNumber;
			pageSize = rowsPerPage;
			pageCount = (int) Math.Ceiling(totalCount/(double) rowsPerPage);
			totalSize = totalCount;

			Set("totalCount", totalCount);
			Set("pageNumber", pageNumber);
			Set("pageSize", pageSize);
			Set("pageCount", pageCount);
		}
		#endregion
		#region Paging Properties
		/// <summary>
		/// Gets a flag indicating whether this dataset is paged or not.
		/// </summary>
		public bool IsPaged { get; private set; }
		/// <summary>
		/// Gets the number of the page currently selected in this dataset.
		/// </summary>
		public int CurrentPage
		{
			get
			{
				// check is paging
				if (!IsPaged)
					throw new InvalidOperationException("This dataset is not paged");
				return currentPage;
			}
		}
		/// <summary>
		/// Gets the total number of pages of this dataset.
		/// </summary>
		/// 
		public int PageCount
		{
			get
			{
				// check is paging
				if (!IsPaged)
					throw new InvalidOperationException("This dataset is not paged");
				return pageCount;
			}
		}
		/// <summary>
		/// Gets the number of rows per page.
		/// </summary>
		public int PageSize
		{
			get
			{
				// check is paging
				if (!IsPaged)
					throw new InvalidOperationException("This dataset is not paged");
				return pageSize;
			}
		}
		/// <summary>
		/// Gets the total number of rows in a paged dataset.
		/// </summary>
		public int TotalSize
		{
			get
			{
				// check is paging
				if (!IsPaged)
					throw new InvalidOperationException("This dataset is not paged");
				return totalSize;
			}
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
		#region Private Fields
		private readonly ICollection<FacetResult> facetResults = new List<FacetResult>();
		private readonly List<IPropertyBag> rows = new List<IPropertyBag>();
		private readonly List<Sort> sorts = new List<Sort>();
		private int currentPage = -1;
		private int pageCount = -1;
		private int pageSize = -1;
		private int totalSize = -1;
		#endregion
	}
}