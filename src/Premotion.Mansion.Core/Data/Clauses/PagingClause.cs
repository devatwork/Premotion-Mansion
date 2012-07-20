using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the query part for paging.
	/// </summary>
	public class PagingClause : NodeQueryClause
	{
		#region Nested type: PagingClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "PagingClause" />.
		/// </summary>
		public class PagingClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public PagingClauseInterpreter(IConversionService conversionService) : base(10)
			{
				// validate arguments
				if (conversionService == null)
					throw new ArgumentNullException("conversionService");

				// set values
				this.conversionService = conversionService;
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// 	Interprets the input.
			/// </summary>
			/// <param name = "context">The <see cref = "IMansionContext" />.</param>
			/// <param name = "input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(IMansionContext context, IPropertyBag input)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (input == null)
					throw new ArgumentNullException("input");

				// check the input
				string pageNumberString;
				var hasPageNumber = input.TryGetAndRemove(context, "pageNumber", out pageNumberString) && pageNumberString.IsNumber();
				string pageSizeString;
				var hasPageSize = input.TryGetAndRemove(context, "pageSize", out pageSizeString) && pageSizeString.IsNumber();
				if (!(hasPageNumber && hasPageSize))
					yield break;

				// get the values
				var pageNumber = conversionService.Convert<int>(context, pageNumberString);
				var pageSize = conversionService.Convert<int>(context, pageSizeString);

				// create the paging clause
				yield return new PagingClause(pageNumber, pageSize);
			}
			#endregion
			#region Private Fields
			private readonly IConversionService conversionService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs a paging clause.
		/// </summary>
		/// <param name = "pageNumber">The number of page</param>
		/// <param name = "pageSize">The page size.</param>
		public PagingClause(int pageNumber, int pageSize)
		{
			// set value
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the page number.
		/// </summary>
		public int PageNumber { get; private set; }
		/// <summary>
		/// 	Gets the page size.
		/// </summary>
		public int PageSize { get; private set; }
		#endregion
		#region Overrides of Object
		/// <summary>
		/// 	Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </summary>
		/// <returns>
		/// 	A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("page-start:{0}&page-size:{1}", PageNumber, PageSize);
		}
		#endregion
	}
}