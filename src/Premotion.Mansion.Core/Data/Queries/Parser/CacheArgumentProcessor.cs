namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Parses the cache parameter.
	/// </summary>
	public class CacheArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public CacheArgumentProcessor() : base(100)
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
			// check for the cache flag
			bool isCachable;
			if (!parameters.TryGetAndRemove(context, "cache", out isCachable))
				isCachable = true;

			// add the query component
			query.Add(new CacheQueryComponent(isCachable));
		}
		#endregion
	}
}