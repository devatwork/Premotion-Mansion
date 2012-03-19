using System;
using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Contains various string builders
	/// </summary>
	public class SqlStringBuilder
	{
		#region Constructors
		/// <summary>
		/// Constructs a SQL string builder.
		/// </summary>
		/// <param name="rootTable"></param>
		public SqlStringBuilder(string rootTable)
		{
			// validate arguments
			if (string.IsNullOrEmpty(rootTable))
				throw new ArgumentNullException("rootTable");

			// set values
			tables.AppendFormat("[{0}]", rootTable);
			includedTables.Add(rootTable);
			this.rootTable = rootTable;
		}
		#endregion
		#region Table Methods
		/// <summary>
		/// Adds the table to the query.
		/// </summary>
		/// <param name="table"></param>
		public void AddTable(Table table)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// check if the table is already included
			if (includedTables.Contains(table.Name))
				return;
			includedTables.Add(table.Name);

			tables.AppendFormat(" INNER JOIN [{0}] ON [{0}].[id] = [{1}].[id]", table.Name, rootTable);
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
			// validate arguments
			if (string.IsNullOrEmpty(clause))
				throw new ArgumentNullException("clause");

			// check for separator
			if (wheres.Length > 0)
				wheres.Append(" AND ");

			
			wheres.Append("(");
			wheres.Append(clause);
			wheres.Append(")");
		}
		/// <summary>
		/// Appends a where clause.
		/// </summary>
		/// <param name="clause">The clause which to append.</param>
		/// <param name="parameters">The parameters to use in the format string.</param>
		public void AppendWhere(string clause, params object[] parameters)
		{
			// validate arguments
			if (string.IsNullOrEmpty(clause))
				throw new ArgumentNullException("clause");

			// check for separator
			if (wheres.Length > 0)
				wheres.Append(" AND ");

			wheres.Append("(");
			wheres.AppendFormat(clause, parameters);
			wheres.Append(")");
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
		/// <param name="clause">The clause which to set.</param>
		/// <param name="parameters">The parameters to use in the format string.</param>
		public void SetLimit(string clause, params object[] parameters)
		{
			// validate arguments
			if (string.IsNullOrEmpty(clause))
				throw new ArgumentNullException("clause");

			// make sure the limit is not set already
			if (!string.IsNullOrEmpty(limit))
				throw new InvalidOperationException("The limit clause can only be set once");

			limit = string.Format(clause, parameters);
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
			if (!string.IsNullOrEmpty(limit))
				commandText.AppendFormat(" {0}", limit);

			if (columns.Length == 0)
				commandText.AppendFormat(" [{0}].* FROM {1}", rootTable, tables);
			else
				commandText.AppendFormat(" {0} FROM {1}", columns, tables);

			if (wheres.Length > 0)
				commandText.AppendFormat(" WHERE {0}", wheres);
			if (orderBys.Length > 0 && OrderByEnabled)
				commandText.AppendFormat(" ORDER BY {0}", orderBys);

			// add the prefix
			if (!string.IsNullOrEmpty(postfix))
				commandText.Append(postfix);

			// close the query
			commandText.AppendLine(";");

			// append the count query
			commandText.Append("SELECT COUNT(1) AS [TotalCount]");
			commandText.AppendFormat(" FROM {0}", tables);
			if (wheres.Length > 0)
				commandText.AppendFormat(" WHERE {0}", wheres);

			// return the assembled query
			return commandText.Replace("@orderBy@", orderBys.ToString()).ToString();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string ToSingleNodeQuery()
		{
			var commandText = new StringBuilder();

			// add the prefix
			if (!string.IsNullOrEmpty(prefix))
				commandText.Append(prefix);

			// assemble the query
			commandText.Append("SELECT");
			if (!string.IsNullOrEmpty(limit))
				commandText.AppendFormat(" {0}", limit);

			if (columns.Length == 0)
				commandText.AppendFormat(" [{0}].* FROM {1}", rootTable, tables);
			else
				commandText.AppendFormat(" {0} FROM {1}", columns, tables);

			if (wheres.Length > 0)
				commandText.AppendFormat(" WHERE {0}", wheres);
			if (orderBys.Length > 0 && OrderByEnabled)
				commandText.AppendFormat(" ORDER BY {0}", orderBys);

			// add the prefix
			if (!string.IsNullOrEmpty(postfix))
				commandText.Append(postfix);

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
			get { return rootTable; }
		}
		#endregion
		#region Private Fields
		private readonly StringBuilder columns = new StringBuilder();
		private readonly ICollection<string> includedTables = new List<string>();
		private readonly StringBuilder orderBys = new StringBuilder();
		private readonly string rootTable;
		private readonly StringBuilder tables = new StringBuilder();
		private readonly StringBuilder wheres = new StringBuilder();
		private string limit;
		private bool orderByEnabled = true;
		private string postfix;
		private string prefix;
		#endregion
	}
}