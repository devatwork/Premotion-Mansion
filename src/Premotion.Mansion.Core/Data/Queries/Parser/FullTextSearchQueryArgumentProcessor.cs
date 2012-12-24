using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Implements the allow roles query argument processor.
	/// </summary>
	public class FullTextSearchQueryArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public FullTextSearchQueryArgumentProcessor() : base(100)
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
			// check for search
			string where;
			if (parameters.TryGetAndRemove(context, "fts", out where) && !string.IsNullOrEmpty(where))
				query.Add(new FullTextSearchSpecification(where));
		}
		#endregion
	}
}