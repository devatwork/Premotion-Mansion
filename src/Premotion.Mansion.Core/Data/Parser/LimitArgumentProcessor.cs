namespace Premotion.Mansion.Core.Data.Parser
{
	/// <summary>
	/// Parses the limit parameter.
	/// </summary>
	public class LimitArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public LimitArgumentProcessor() : base(100)
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
			int limit;
			if (!parameters.TryGetAndRemove(context, "limit", out limit))
				return;

			// parse the sorts and add them to the query
			query.Add(new LimitQueryComponent(limit));
		}
		#endregion
	}
}