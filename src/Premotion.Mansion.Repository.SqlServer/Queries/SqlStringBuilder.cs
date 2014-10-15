using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Contains various string builders
	/// </summary>
	public class SqlStringBuilder
	{
		#region Constants
		/// <summary>
		/// This value will be replaced with the FROM caluse when the query is turned into a string.
		/// </summary>
		public const string FromReplacePlaceholder = "@@@FROM@@@";
		/// <summary>
		/// This value will be replaced with the WHERE clause when the query is turned into a string.
		/// </summary>
		public const string WhereReplacePlaceholder = "@@@WHERE@@@";
		/// <summary>
		/// This value will be replaced with the ORDER BY clause when the query is turned into a string.
		/// </summary>
		public const string OrderByReplacePlaceholder = "@@@ORDERBY@@@";
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a SQL string builder.
		/// </summary>
		/// <param name="rootTable"></param>
		public SqlStringBuilder(Table rootTable)
		{
			// validate arguments
			if (rootTable == null)
				throw new ArgumentNullException("rootTable");

			// set values
			tables.AppendFormat("[{0}]", rootTable.Name);
			includedTables.Add(rootTable);
			this.rootTable = rootTable;
		}
		#endregion
		#region Table Methods
		/// <summary>
		/// Adds the table to the query.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="command">The <see cref="SqlCommand"/>.</param>
		/// <param name="table">The <see cref="Table"/> which to add.</param>
		public void AddTable(IMansionContext context, Table table, SqlCommand command)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (table == null)
				throw new ArgumentNullException("table");
			if (command == null)
				throw new ArgumentNullException("command");

			// check if the table is already included
			if (includedTables.Contains(table))
				return;
			includedTables.Add(table);

			tables.Append(" " + table.ToJoinStatement(context, rootTable, command));
		}
		#endregion
		#region Clause Methods
		/// <summary>
		/// Appends a column clause.
		/// </summary>
		/// <param name="clause">The clause which to append.</param>
		public void AppendColumn(string clause)
		{
			// validate arguments
			if (string.IsNullOrEmpty(clause))
				throw new ArgumentNullException("clause");

			// check for separator
			if (columns.Length > 0)
				columns.Append(", ");

			columns.Append(clause);
		}
		/// <summary>
		/// Appends a column clause.
		/// </summary>
		/// <param name="clause">The clause which to append.</param>
		/// <param name="parameters">The parameters to use in the format string.</param>
		public void AppendColumn(string clause, params object[] parameters)
		{
			// validate arguments
			if (string.IsNullOrEmpty(clause))
				throw new ArgumentNullException("clause");

			// check for separator
			if (columns.Length > 0)
				columns.Append(", ");

			columns.AppendFormat(clause, parameters);
		}
		/// <summary>
		/// Appends a where clause.
		/// </summary>
		/// <param name="clause">The clause which to append.</param>
		public void AppendWhere(string clause)
		{
			WhereBuilder.AppendWhere(clause);
		}
		/// <summary>
		/// Appends a where clause.
		/// </summary>
		/// <param name="clause">The clause which to append.</param>
		/// <param name="parameters">The parameters to use in the format string.</param>
		public void AppendWhere(string clause, params object[] parameters)
		{
            WhereBuilder.AppendWhere(clause, parameters);
		}
		/// <summary>
		/// Appends a ORDER BY clause.
		/// </summary>
		/// <param name="clause">The clause which to append.</param>
		public void AppendOrderBy(string clause)
		{
			// validate arguments
			if (string.IsNullOrEmpty(clause))
				throw new ArgumentNullException("clause");

			// check for separator
			if (orderBys.Length > 0)
				orderBys.Append(", ");

			orderBys.Append(clause);
		}
		/// <summary>
		/// Sets the limit clause.
		/// </summary>
		/// <param name="maxNumberOfResults">The parameter containing the limit.</param>
		public void SetLimit(int maxNumberOfResults)
		{
			// make sure the limit is not set already
			if (limit.HasValue)
				throw new InvalidOperationException("The limit clause can only be set once");

			limit = maxNumberOfResults;
		}
		/// <summary>
		/// Sets the prefix.
		/// </summary>
		/// <param name="clause">The clause which to set.</param>
		public void SetPrefix(string clause)
		{
			// validate arguments
			if (string.IsNullOrEmpty(clause))
				throw new ArgumentNullException("clause");

			// make sure the prefix is not set already
			if (!string.IsNullOrEmpty(prefix))
				throw new InvalidOperationException("The prefix clause can only be set once");

			prefix = clause;
		}
		/// <summary>
		/// Sets the postfix.
		/// </summary>
		/// <param name="clause">The clause which to set.</param>
		/// <param name="parameters">The parameters to use in the format string.</param>
		public void SetPostfix(string clause, params object[] parameters)
		{
			// validate arguments
			if (string.IsNullOrEmpty(clause))
				throw new ArgumentNullException("clause");

			// make sure the postfix is not set already
			if (!string.IsNullOrEmpty(postfix))
				throw new InvalidOperationException("The postfix clause can only be set once");

			postfix = string.Format(clause, parameters);
		}
		#endregion
		#region Additional Query Methods
		/// <summary>
		/// Adds an addional query to this query.
		/// </summary>
		/// <param name="query">The additional query which to add.</param>
		/// <param name="mapper">The mapper which to use to map the results to the query.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		public void AddAdditionalQuery(string query, Action<RecordSet, IDataReader> mapper)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");
			if (mapper == null)
				throw new ArgumentNullException("mapper");

			// add the additional query
			additionalQueries.Append(query);
			additionalQueries.AppendLine(";");
			additionalQueryMappers.Enqueue(mapper);
		}
		/// <summary>
		/// Gets the additional query result mapper.
		/// </summary>
		/// <returns>Returns the next addional query result mapper.</returns>
		public Action<RecordSet, IDataReader> GetAdditionalQueryResultMapper()
		{
			return additionalQueryMappers.Dequeue();
		}
		#endregion
		#region ToString Methods
		/// <summary>
		/// Converts this query string builder to a string.
		/// </summary>
		/// <returns>Returns the string.</returns>
		public override string ToString()
		{
			var commandText = new StringBuilder();

			// add the prefix
			if (!string.IsNullOrEmpty(prefix))
				commandText.Append(prefix);

			// assemble the query
			commandText.Append("SELECT");
			if (limit.HasValue)
				commandText.AppendFormat(" TOP {0}", limit);

			if (columns.Length == 0)
				commandText.AppendFormat(" [{0}].* {1}", rootTable.Name, FromReplacePlaceholder);
			else
				commandText.AppendFormat(" {0} {1}", columns, FromReplacePlaceholder);

			commandText.AppendFormat(WhereReplacePlaceholder);
			if (OrderByEnabled)
				commandText.Append(OrderByReplacePlaceholder);

			// add the prefix
			if (!string.IsNullOrEmpty(postfix))
				commandText.Append(postfix);

			// close the query
			commandText.AppendLine(";");

			// append the count query
			commandText.Append("SELECT ");
			if (limit.HasValue)
				commandText.AppendFormat("Case When COUNT(1) < {0} Then COUNT(1) Else {0} End", limit);
			else
				commandText.Append("COUNT(1)");
			commandText.Append(" AS [TotalCount]");
			commandText.Append(FromReplacePlaceholder);
			commandText.Append(WhereReplacePlaceholder);
			commandText.AppendLine(";");

			// append the additional queries
			commandText.Append(additionalQueries);

			// replace values
			commandText.Replace(FromReplacePlaceholder, string.Format(" FROM {0}", tables));
			commandText.Replace(WhereReplacePlaceholder, WhereBuilder.Buffer.Length > 0 ? string.Format(" WHERE {0}", WhereBuilder.Buffer) : string.Empty);
			commandText.Replace(OrderByReplacePlaceholder, orderBys.Length > 0 ? string.Format(" ORDER BY {0}", orderBys) : string.Empty);

			// return the assembled query
			return commandText.ToString();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string ToSingleNodeQuery()
		{
			// construct the buffer
			var commandText = new StringBuilder();

			// add the prefix
			if (!string.IsNullOrEmpty(prefix))
				commandText.Append(prefix);

			// assemble the query
			commandText.Append("SELECT");
			if (limit.HasValue)
				commandText.AppendFormat(" TOP {0}", limit);

			if (columns.Length == 0)
				commandText.AppendFormat(" [{0}].* {1}", rootTable.Name, FromReplacePlaceholder);
			else
				commandText.AppendFormat(" {0} {1}", columns, FromReplacePlaceholder);

			commandText.AppendFormat(WhereReplacePlaceholder);
			if (OrderByEnabled)
				commandText.Append(OrderByReplacePlaceholder);

			// add the prefix
			if (!string.IsNullOrEmpty(postfix))
				commandText.Append(postfix);

			// replace values
			commandText.Replace(FromReplacePlaceholder, string.Format(" FROM {0}", tables));
			commandText.Replace(WhereReplacePlaceholder, WhereBuilder.Buffer.Length > 0 ? string.Format(" WHERE {0}", WhereBuilder.Buffer) : string.Empty);
			commandText.Replace(OrderByReplacePlaceholder, orderBys.Length > 0 ? string.Format(" ORDER BY {0}", orderBys) : string.Empty);

			return commandText.ToString();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets/Sets a flag indicating whether the order by should be used in the regular statement.
		/// </summary>
		public bool OrderByEnabled
		{
			private get { return orderByEnabled; }
			set { orderByEnabled = value; }
		}
		/// <summary>
		/// Gets the root table name.
		/// </summary>
		public string RootTableName
		{
			get { return rootTable.Name; }
		}
		/// <summary>
		/// Gets all the <see cref="Table"/>s asociated with this query.
		/// </summary>
		public IEnumerable<Table> Tables
		{
			get { return includedTables; }
		}
		/// <summary>
		/// Gets the active where clause <see cref="StringBuilder"/>/
		/// </summary>
        private SqlQueryAggregator WhereBuilder
		{
			get
			{
                SqlQueryAggregator current;
				return whereBuilderStack.TryPeek(out current) ? current : rootWhereBuilder;
			}
		}
		/// <summary>
		/// Gets the where builder <see cref="IAutoPopStack{TEntry}"/>.
		/// </summary>
		public IAutoPopStack<SqlQueryAggregator> WhereBuilderStack
		{
			get { return whereBuilderStack; }
		}
		#endregion
		#region Private Fields
		private readonly StringBuilder additionalQueries = new StringBuilder();
		private readonly Queue<Action<RecordSet, IDataReader>> additionalQueryMappers = new Queue<Action<RecordSet, IDataReader>>();
		private readonly StringBuilder columns = new StringBuilder();
		private readonly ICollection<Table> includedTables = new List<Table>();
		private readonly StringBuilder orderBys = new StringBuilder();
		private readonly Table rootTable;
        private readonly SqlQueryAggregator rootWhereBuilder = SqlQueryAggregator.And();
		private readonly StringBuilder tables = new StringBuilder();
        private readonly AutoPopStack<SqlQueryAggregator> whereBuilderStack = new AutoPopStack<SqlQueryAggregator>();
		private int? limit;
		private bool orderByEnabled = true;
		private string postfix;
		private string prefix;
		#endregion
	}
}