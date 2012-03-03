using System;
using System.Collections.Generic;

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
		/// <param name="totalRowCount">The total number of rows in the complete set.</param>
		/// <param name="pageNumber">The current page this dataset displays.</param>
		/// <param name="rowsPerPage">The number of rows per page.</param>
		protected void SetPaging(int totalRowCount, int pageNumber, int rowsPerPage)
		{
			IsPaged = true;
			currentPage = pageNumber;
			pageSize = rowsPerPage;
			pageCount = (int) Math.Ceiling(totalRowCount/(double) rowsPerPage);
			totalSize = totalRowCount;
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
		#region Private Fields
		private readonly List<IPropertyBag> rows = new List<IPropertyBag>();
		private readonly List<Sort> sorts = new List<Sort>();
		private int currentPage = -1;
		private int pageCount = -1;
		private int pageSize = -1;
		private int totalSize = -1;
		#endregion
	}
}