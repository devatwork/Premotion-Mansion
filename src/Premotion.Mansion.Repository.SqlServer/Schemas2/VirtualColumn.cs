using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// A dummy column.
	/// </summary>
	public class VirtualColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="columnName"></param>
		public VirtualColumn(string columnName) : base(columnName)
		{
		}
		#endregion
		#region Overrides of Column
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		protected override void DoToWhereStatement(IMansionContext context, QueryCommandContext commandContext, IEnumerable<object> values)
		{
			throw new NotSupportedException();
		}
		#endregion
	}
}