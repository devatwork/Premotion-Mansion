using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Parses the sort parameter.
	/// </summary>
	public class SortArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public SortArgumentProcessor() : base(100)
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
			string sortString;
			if (!parameters.TryGetAndRemove(context, "sort", out sortString) || string.IsNullOrEmpty(sortString))
				return;

			// parse the sorts and add them to the query
			query.Add(Sort.Parse(sortString));
		}
		#endregion
	}
}