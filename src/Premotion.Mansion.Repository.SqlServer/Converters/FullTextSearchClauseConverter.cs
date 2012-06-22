using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Patterns.Tokenizing;
using Premotion.Mansion.Repository.SqlServer.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="FullTextSearchClause"/> into a SQL query statement.
	/// </summary>
	public class FullTextSearchClauseConverter : ClauseConverter<FullTextSearchClause>
	{
		#region Nested type: FullTextSearchTable
		/// <summary>
		/// Represents a full-text search table.
		/// </summary>
		private class FullTextSearchTable : Table
		{
			#region Constructors
			/// <summary>
			/// Constructs a full-text search table.
			/// </summary>
			/// <param name="foreignTableName">The name of the table to which to join.</param>
			/// <param name="searchCondition">The condition on which to search.</param>
			/// <param name="tableName">The name of the table which to search.</param>
			/// <param name="columnNames">The name of the columns which to search, default is all columns (*).</param>
			/// <param name="top">The maximum number of results which to return.</param>
			/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
			public FullTextSearchTable(string foreignTableName, string searchCondition, string tableName, string columnNames = "*", int top = 500) : base("FTS")
			{
				// validate arguments
				if (string.IsNullOrEmpty(foreignTableName))
					throw new ArgumentNullException("foreignTableName");
				if (string.IsNullOrEmpty(searchCondition))
					throw new ArgumentNullException("searchCondition");
				if (string.IsNullOrEmpty(tableName))
					throw new ArgumentNullException("tableName");

				// set values
				this.foreignTableName = foreignTableName;
				this.searchCondition = searchCondition;
				this.tableName = tableName;
				this.columnNames = columnNames;
				this.top = top;
			}
			#endregion
			#region Overrides of Table
			/// <summary>
			/// Generates a statement which joins this table to the given <paramref name="rootTable"/>/
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <param name="rootTable">The root <see cref="Table"/> to which to join this table.</param>
			/// <param name="command">The <see cref="SqlCommand"/>.</param>
			/// <returns>Returns the join statement.</returns>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="rootTable"/> is null.</exception>
			protected override string DoToJoinStatement(IMansionContext context, Table rootTable, SqlCommand command)
			{
				return string.Format("INNER JOIN CONTAINSTABLE({0}, {1}, {2}, @{3}) AS [{4}] ON [{4}].[KEY] = [{5}].[id]", tableName, columnNames, searchCondition, command.AddParameter(top), Name, foreignTableName);
			}
			#endregion
			#region Private Fields
			private readonly string columnNames;
			private readonly string foreignTableName;
			private readonly string searchCondition;
			private readonly string tableName;
			private readonly int top;
			#endregion
		}
		#endregion
		#region Nested type: QueryTokenizer
		/// <summary>
		/// Tokenizes the query parameters.
		/// </summary>
		private class QueryTokenizer : ITokenizer<string, string>
		{
			#region Implementation of ITokenizer<in string,out string>
			/// <summary>
			/// Tokenizes the input.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <param name="input">The input for this tokenizer.</param>
			/// <returns>Returns the tokens parsed from <paramref name="input"/>.</returns>
			public IEnumerable<string> Tokenize(IMansionContext context, string input)
			{
				// loop through all the charachter in the input
				var buffer = new StringBuilder();
				var inLiteral = false;
				foreach (var chararcter in input)
				{
					// if the current character is a quote, toggle the inLiteral flag
					if (chararcter == '"')
					{
						//  toggle the literal flag
						inLiteral = !inLiteral;

						// if the literal is finished, return the content if the buffer, if any
						if (buffer.Length > 0)
						{
							yield return buffer.ToString().Trim();
							buffer.Length = 0;
						}

						// eat literals because they are yummie
						continue;
					}

					// if the current character is a whitespace and we are not within a literal
					if (Char.IsWhiteSpace(chararcter) && !inLiteral)
					{
						// return the content of the buffer, if any
						if (buffer.Length > 0)
						{
							yield return buffer.ToString().Trim();
							buffer.Length = 0;
						}

						// eat the whitespace, it is yummie
						continue;
					}

					// add the character to the buffer
					buffer.Append(chararcter);
				}

				// return the remainder, if any
				if (buffer.Length > 0)
					yield return buffer.ToString().Trim();
			}
			#endregion
		}
		#endregion
		#region Map Methods
		/// <summary>
		/// Maps this clause to a SQL query.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <param name="command"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="clause">The clause.</param>
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, FullTextSearchClause clause)
		{
			// process the query
			var tokens = Tokenizer.Tokenize(context, clause.Query);
			var conditions = tokens.Select(token => string.Format("(\"{0}\" OR \"{0}*\")", token));
			var condition = string.Join(" AND ", conditions);

			// create the condition parameter
			var conditionParameter = command.AddParameter(condition);

			// build the full text table
			var fullTextTable = new FullTextSearchTable(queryBuilder.RootTableName, "@" + conditionParameter, queryBuilder.RootTableName);

			// add the full text table to the query
			queryBuilder.AddTable(context, fullTextTable, command);

			// add a sort clause
			queryBuilder.AppendOrderBy(string.Format("[{0}].[RANK] DESC", fullTextTable.Name));
		}
		#endregion
		#region Private Fields
		private static readonly QueryTokenizer Tokenizer = new QueryTokenizer();
		#endregion
	}
}