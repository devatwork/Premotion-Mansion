namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Parses the paging parameters.
	/// </summary>
	public class PagingArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public PagingArgumentProcessor() : base(100)
		{
		}
		#endregion
		#region Overrides of QueryArgumentProcessor
		/// <summary>
		/// Processes the <paramref name="parameters"/> and turn them into <paramref name="query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parameters">The parameters which to process.</param>
		/// <param name="query">The <see cref="Query"/> in which to set the parameters.</param>
		protected override void DoProcess(IMansionContext context, IPropertyBag parameters, Query query)
		{
			// check the input
			int pageNumber;
			var hasPageNumber = parameters.TryGetAndRemove(context, "pageNumber", out pageNumber);
			int pageSize;
			var hasPageSize = parameters.TryGetAndRemove(context, "pageSize", out pageSize);

			// add the paging query component
			if ((hasPageNumber && hasPageSize))
				query.Add(new PagingQueryComponent(pageNumber, pageSize));
		}
		#endregion
	}
}