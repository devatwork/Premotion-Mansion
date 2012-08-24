using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Allows to build a modification query.
	/// </summary>
	public class ModificationQueryBuilder
	{
		#region Constants
		private const char TableOpen = '[';
		private const char TableClose = ']';
		private const char ColumnOpen = '[';
		private const char ColumnClose = ']';
		private const char QueryEnd = ';';
		private const char ParameterNamePrefix = '@';
		private static readonly char[] SubQuerySeperators = new[] {';', ' '};
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a modification query around the <paramref name="command"/>.
		/// </summary>
		/// <param name="command">The <see cref="IDbCommand"/>.</param>
		public ModificationQueryBuilder(IDbCommand command)
		{
			// validate arguments
			if (command == null)
				throw new ArgumentNullException("command");

			// set values
			this.command = command;
		}
		/// <summary>
		/// Constructs a modification query with a <paramref name="parentQueryBuilder"/>. This query builder does not have its own command.
		/// </summary>
		/// <param name="parentQueryBuilder">The <see cref="ModificationQueryBuilder"/>.</param>
		public ModificationQueryBuilder(ModificationQueryBuilder parentQueryBuilder)
		{
			// validate arguments
			if (parentQueryBuilder == null)
				throw new ArgumentNullException("parentQueryBuilder");

			// set values
			this.parentQueryBuilder = parentQueryBuilder;
		}
		#endregion
		#region Append/Prepend Methods
		/// <summary>
		/// Prepends this modification query with <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The query wich to prepent.</param>
		public void PrependQuery(string query)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");

			// trim
			query = query.Trim(SubQuerySeperators);

			// add the query
			prependQueries.Enqueue(query);
		}
		/// <summary>
		/// Appengs this modification query with <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The query wich to append.</param>
		public void AppendQuery(string query)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");

			// trim
			query = query.Trim(SubQuerySeperators);

			// add the query
			appendQueries.Enqueue(query);
		}
		#endregion
		#region Column Methods
		/// <summary>
		/// Adds a value for an particular column.
		/// </summary>
		/// <param name="columnName">The name of the column which to modify.</param>
		/// <param name="valueParameterName">The name of the parameter containing the new column value.</param>
		public void AddColumnValue(string columnName, string valueParameterName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");
			if (string.IsNullOrEmpty(valueParameterName))
				throw new ArgumentNullException("valueParameterName");

			// value parameters always start with the ParameterNamePrefix
			if (!ParameterNamePrefix.Equals(valueParameterName[0]))
				throw new InvalidOperationException(string.Format("Value parameter name '{0}' is invalid, it should start with '{1}'", valueParameterName, ParameterNamePrefix));

			// add check if the column is not already set with a value parameter
			if (modifiedColumns.ContainsKey(columnName))
				throw new InvalidOperationException(string.Format("Column '{0}' was already modified during this query.", columnName));

			// add the column to the list
			modifiedColumns.Add(columnName, valueParameterName);
		}
		/// <summary>
		/// Adds a <paramref name="value"/> for an particular column.
		/// </summary>
		/// <param name="columnName">The name of the column which to modify.</param>
		/// <param name="value">The value of the parameter. When null it will be transformed to <see cref="DBNull.Value"/>.</param>
		/// <param name="type">The <see cref="DbType"/> of the <paramref name="value"/></param>
		public void AddColumnValue(string columnName, object value, DbType? type = null)
		{
			// validate arguments
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");

			// get the value parameter name
			var valueParameterName = AddParameter(columnName, value, type);

			// add the column value
			AddColumnValue(columnName, valueParameterName);
		}
		#endregion
		#region Where Clause Methods
		/// <summary>
		/// Appens a where clause to this query builder.
		/// </summary>
		/// <param name="whereClause">The where clause.</param>
		public void AppendWhereClause(string whereClause)
		{
			// validate arguments
			if (string.IsNullOrEmpty(whereClause))
				throw new ArgumentNullException("whereClause");

			// add the clause
			whereClauses.Enqueue(whereClause);
		}
		#endregion
		#region Parameter Methods
		/// <summary>
		/// Adds a parameter to this query.
		/// </summary>
		/// <param name="name">The name of the parameter which to add.</param>
		/// <param name="value">The value of the parameter. When null it will be transformed to <see cref="DBNull.Value"/>.</param>
		/// <param name="type">The <see cref="DbType"/> of the parameter.</param>
		/// <returns>Returns the name of the parameter which might be different from <paramref name="name"/>.</returns>
		public string AddParameter(string name, object value, DbType? type = null)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// generate the name of the parameter
			var parameterName = name + Command.Parameters.Count;

			// add the parameter
			var parameter = Command.CreateParameter();
			parameter.ParameterName = parameterName;
			parameter.Value = (value ?? DBNull.Value);
			if (type.HasValue)
				parameter.DbType = type.Value;
			parameter.Direction = ParameterDirection.Input;
			Command.Parameters.Add(parameter);

			// return the name of the parameter including prefix
			return ParameterNamePrefix + parameterName;
		}
		#endregion
		#region To Statements Methods
		/// <summary>
		/// Turns this modification query builder into an insert statement.
		/// </summary>
		/// <returns></returns>
		public string ToInsertStatement(string tableName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(tableName))
				throw new ArgumentNullException("tableName");

			// create the statement buffer
			var statement = new StringBuilder();

			// first add all the prepend queries
			AppendPrependedStatements(statement);

			// second if there are modified columns add them
			if (HasModifiedColumns)
			{
				// start the insert statement
				statement.AppendFormat("INSERT INTO {0}{1}{2} (", TableOpen, tableName, TableClose);

				// add the column names
				var columnIndex = 1;
				foreach (var name in modifiedColumns.Keys)
				{
					statement.AppendFormat("{0}{1}{2}", ColumnOpen, name, ColumnClose);
					if (columnIndex == modifiedColumns.Count)
						break;
					statement.Append(", ");
					columnIndex++;
				}

				// half way through
				statement.Append(") VALUES (");
				var valueIndex = 1;
				foreach (var value in modifiedColumns.Values)
				{
					statement.Append(value);
					if (valueIndex == modifiedColumns.Count)
						break;
					statement.Append(", ");
					valueIndex++;
				}

				// finish the query
				statement.AppendLine(")" + QueryEnd);
			}

			// third add all the appended queries
			AppendAppendedStatements(statement);

			// return the constructed statement
			return statement.ToString();
		}
		/// <summary>
		/// Turns this modification query builder into an update statement.
		/// </summary>
		/// <returns></returns>
		public string ToUpdateStatement(string tableName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(tableName))
				throw new ArgumentNullException("tableName");

			// create the statement buffer
			var statement = new StringBuilder();

			// first add all the prepend queries
			AppendPrependedStatements(statement);

			// second generate the update statement
			if (HasModifiedColumns)
			{
				// append the update query
				statement.AppendFormat("UPDATE {0}{1}{2} SET ", TableOpen, tableName, TableClose);

				// append the updated columns
				var columnIndex = 1;
				foreach (var column in modifiedColumns)
				{
					statement.AppendFormat("{0}{1}{2} = {3}", ColumnOpen, column.Key, ColumnClose, column.Value);
					if (columnIndex == modifiedColumns.Count)
						break;
					statement.Append(", ");
					columnIndex++;
				}

				//  append the where clauses
				AppendWhereClauses(statement);
			}

			// third add all the appended queries
			AppendAppendedStatements(statement);

			// return the constructed statement
			return statement.ToString();
		}
		/// <summary>
		/// Turns this modification query builder into an delete statement.
		/// </summary>
		/// <returns></returns>
		public string ToDeleteStatement(string tableName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(tableName))
				throw new ArgumentNullException("tableName");

			// create the statement buffer
			var statement = new StringBuilder();

			// first add all the prepend queries
			AppendPrependedStatements(statement);

			// second generate the delete statement
			statement.AppendFormat("DELETE FROM {0}{1}{2}", TableOpen, tableName, TableClose);
			AppendWhereClauses(statement);
			statement.AppendLine(QueryEnd.ToString());

			// third add all the appended queries
			AppendAppendedStatements(statement);

			// return the constructed statement
			return statement.ToString();
		}
		/// <summary>
		/// Turns this modification query builder into an statement.
		/// </summary>
		/// <returns></returns>
		public string ToStatement()
		{
			// create the statement buffer
			var statement = new StringBuilder();

			// first add all the prepend queries
			AppendPrependedStatements(statement);

			// second add all the appended queries
			AppendAppendedStatements(statement);

			// return the constructed statement
			return statement.ToString();
		}
		/// <summary>
		/// Appends the <see cref="prependQueries"/> to the <paramref name="statement"/>.
		/// </summary>
		/// <param name="statement"></param>
		private void AppendPrependedStatements(StringBuilder statement)
		{
			foreach (var subQuery in prependQueries)
				statement.AppendLine(subQuery + QueryEnd);
		}
		/// <summary>
		/// Appends the <see cref="whereClauses"/> to the <paramref name="statement"/>.
		/// </summary>
		/// <param name="statement"></param>
		private void AppendWhereClauses(StringBuilder statement)
		{
			if (whereClauses.Count <= 0)
				return;

			statement.Append(" WHERE ");

			// add the clauses
			var clauseIndex = 1;
			foreach (var clause in whereClauses)
			{
				statement.AppendFormat("({0})", clause);
				if (clauseIndex == whereClauses.Count)
					break;
				statement.Append(" AND ");
				clauseIndex++;
			}
		}
		/// <summary>
		/// Appends the <see cref="appendQueries"/> to the <paramref name="statement"/>.
		/// </summary>
		/// <param name="statement"></param>
		private void AppendAppendedStatements(StringBuilder statement)
		{
			foreach (var subQuery in appendQueries)
				statement.AppendLine(subQuery + QueryEnd);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the parent <see cref="ModificationQueryBuilder"/>.
		/// </summary>
		private ModificationQueryBuilder ParentQueryBuilder
		{
			get
			{
				if (parentQueryBuilder == null)
					throw new InvalidOperationException("This query builder does not have a parent query builder");
				return parentQueryBuilder;
			}
		}
		/// <summary>
		/// Gets the <see cref="IDbCommand"/>.
		/// </summary>
		public IDbCommand Command
		{
			get { return command ?? ParentQueryBuilder.Command; }
		}
		/// <summary>
		/// Gets a flag whether this modification query builder holds modified columns.
		/// </summary>
		public bool HasModifiedColumns
		{
			get { return modifiedColumns.Count > 0; }
		}
		#endregion
		#region Private Fields
		private readonly Queue<string> appendQueries = new Queue<string>();
		private readonly IDbCommand command;
		private readonly Dictionary<string, string> modifiedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		private readonly ModificationQueryBuilder parentQueryBuilder;
		private readonly Queue<string> prependQueries = new Queue<string>();
		private readonly Queue<string> whereClauses = new Queue<string>();
		#endregion
	}
}