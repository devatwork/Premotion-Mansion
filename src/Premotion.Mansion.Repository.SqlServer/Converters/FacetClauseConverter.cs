using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="FacetClause"/> into a SQL query statement.
	/// </summary>
	public class FacetClauseConverter : ClauseConverter<FacetClause>
	{
		#region Overrides of ClauseConverter<FacetClause>
		/// <summary>
		/// Maps this clause to a SQL query.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <param name="command"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="clause">The clause.</param>
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, FacetClause clause)
		{
			// find the property on which to facet
			var columnAndTable = schema.FindTableAndColumn(clause.Facet.PropertyName);

			// build the query
			var queryBuffer = new StringBuilder("SELECT");

			// append the columns
			queryBuffer.AppendFormat(" [{0}], COUNT(1) FROM [{1}]", columnAndTable.Column.ColumnName, columnAndTable.Table.Name);

			// append the where statement
			queryBuffer.AppendFormat(" WHERE ( [{0}].[id] IN( SELECT [{1}].[id]{2}{3} ) )", columnAndTable.Table.Name, schema.RootTable.Name, SqlStringBuilder.FromReplacePlaceholder, SqlStringBuilder.WhereReplacePlaceholder);

			// if the table is a multi-valued property table, add the property name clause
			if (columnAndTable.Table is MultiValuePropertyTable)
				queryBuffer.AppendFormat(" AND ( [name] = @{0} )", command.AddParameter(columnAndTable.Column.PropertyName));

			// append the group clause
			queryBuffer.AppendFormat(" GROUP BY [{0}]", columnAndTable.Column.ColumnName);

			// add with custom mapping to the query
			queryBuilder.AddAdditionalQuery(queryBuffer.ToString(), (nodeset, reader) =>
			                                                        {
			                                                        	// loop over all the records to create the facet values
			                                                        	var facetValues = new List<FacetValue>();
			                                                        	while (reader.Read())
			                                                        	{
			                                                        		// get the value
			                                                        		var value = reader.GetValue(0);
			                                                        		var count = reader.GetInt32(1);

			                                                        		// construct the facet value
			                                                        		var facetValue = new FacetValue(value, count);

			                                                        		// add the value to the list
			                                                        		facetValues.Add(facetValue);
			                                                        	}

			                                                        	// create the facet result
			                                                        	var result = FacetResult.Create(context, clause.Facet, facetValues);

			                                                        	// add the result to the facet set
			                                                        	nodeset.AddFacet(result);
			                                                        });
		}
		#endregion
	}
}