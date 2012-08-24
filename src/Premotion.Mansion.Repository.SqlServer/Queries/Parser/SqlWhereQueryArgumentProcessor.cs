using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Parser;
using Premotion.Mansion.Repository.SqlServer.Queries.Specifications;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Parser
{
	/// <summary>
	/// Implements the SQL-server where <see cref="QueryArgumentProcessor"/>.
	/// </summary>
	public class SqlWhereQueryArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public SqlWhereQueryArgumentProcessor() : base(100)
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
			if (parameters.TryGetAndRemove(context, "sqlWhere", out where) && !string.IsNullOrEmpty(where))
				query.Add(new SqlWhereSpecification(where));
		}
		#endregion
	}
}