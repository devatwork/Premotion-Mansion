using System.Text;

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
		#region Overrides of QueryComponent
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.AppendFormat("page-start:{0}&page-size:{1}", PageNumber, PageSize);
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