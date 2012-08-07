namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Represents a paging <see cref="QueryComponent"/>.
	/// </summary>
	public class PagingQueryComponent : QueryComponent
	{
		#region Constructors
		/// <summary>
		/// Constructs a paging clause.
		/// </summary>
		/// <param name="pageNumber">The number of page</param>
		/// <param name="pageSize">The page size.</param>
		public PagingQueryComponent(int pageNumber, int pageSize)
		{
			// set value
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the page number.
		/// </summary>
		public int PageNumber { get; private set; }
		/// <summary>
		/// Gets the page size.
		/// </summary>
		public int PageSize { get; private set; }
		#endregion
	}
}