using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;

namespace Premotion.Mansion.Core.Data.Queries.Parser.Nodes
{
	/// <summary>
	/// Implements the status query argument processor.
	/// </summary>
	public class StatusQueryArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public StatusQueryArgumentProcessor() : base(100)
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
			string status;
			if (parameters.TryGetAndRemove(context, "status", out status) && !string.IsNullOrEmpty(status))
				query.Add(StatusSpecification.Is(status));
		}
		#endregion
	}
}