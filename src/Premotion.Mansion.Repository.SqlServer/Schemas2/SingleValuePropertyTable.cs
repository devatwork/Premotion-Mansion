using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a multi-value property table.
	/// </summary>
	public class SingleValuePropertyTable : Table
	{
		#region Nested type: SingleValuePropertyColumn
		/// <summary>
		/// Implements <see cref="Column"/> for single value properties.
		/// </summary>
		private class SingleValuePropertyColumn : Column
		{
			#region constructors
			/// <summary>
			/// </summary>
			/// <param name="columnName"></param>
			public SingleValuePropertyColumn(string columnName) : base("value", columnName)
			{
			}
			#endregion
			#region Overrides of Column
			/// <summary>
			/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
			/// <param name="pair">The <see cref="TableColumnPair"/>.</param>
			/// <param name="values">The values on which to construct the where statement.</param>
			protected override void DoToWhereStatement(IMansionContext context, QueryCommandContext commandContext, TableColumnPair pair, IList<object> values)
			{
				// assemble the properties
				var buffer = new StringBuilder();
				foreach (var value in values)
					buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(value));

				// append the query
				commandContext.QueryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) )", commandContext.QueryBuilder.RootTableName, pair.Table.Name, pair.Column.ColumnName, buffer.Trim());
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <param name="queryBuilder"></param>
			/// <param name="newPointer"></param>
			/// <param name="properties"></param>
			protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag properties)
			{
				throw new NotSupportedException();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this table with the given <paramref name="tableName"/>.
		/// </summary>
		/// <param name="tableName">The name of this table.</param>
		/// <param name="propertyName">The name of the property which to store.</param>
		public SingleValuePropertyTable(string tableName, string propertyName) : base(tableName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// add a column
			Add(new SingleValuePropertyColumn(propertyName));

			// set value
			PropertyName = propertyName;
		}
		#endregion
		#region Overrides of Table
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="newProperties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			// check if there are any properties
			var values = newProperties.Get(context, PropertyName, string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
			if (values.Length == 0)
				return;

			// loop through each value and write an insert statement
			foreach (var value in values)
			{
				// build the query
				var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

				// set column values
				valueModificationQuery.AddColumnValue("id", "@ScopeIdentity");
				valueModificationQuery.AddColumnValue("value", value, DbType.String);

				// append the query
				queryBuilder.AppendQuery(valueModificationQuery.ToInsertStatement(Name));
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property which to store.
		/// </summary>
		private string PropertyName { get; set; }
		#endregion
	}
}