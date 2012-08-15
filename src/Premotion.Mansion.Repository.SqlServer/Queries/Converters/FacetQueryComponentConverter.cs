using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="FacetQueryComponent"/>s.
	/// </summary>
	public class FacetQueryComponentConverter : QueryComponentConverter<FacetQueryComponent>
	{
		#region Overrides of QueryComponentConverter<FacetQueryComponent>
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, FacetQueryComponent component, QueryCommandContext commandContext)
		{
			// find the property on which to facet
			var columnAndTable = commandContext.Schema.FindTableAndColumn(component.Facet.PropertyName);

			// build the query
			var queryBuffer = new StringBuilder("SELECT");

			// append the columns
			queryBuffer.AppendFormat(" [{0}], COUNT(1) FROM [{1}]", columnAndTable.Column.ColumnName, columnAndTable.Table.Name);

			// append the where statement
			queryBuffer.AppendFormat(" WHERE ( [{0}].[id] IN( SELECT [{1}].[id]{2}{3} ) )", columnAndTable.Table.Name, commandContext.Schema.RootTable.Name, SqlStringBuilder.FromReplacePlaceholder, SqlStringBuilder.WhereReplacePlaceholder);

			// if the table is a multi-valued property table, add the property name clause
			if (columnAndTable.Table is MultiValuePropertyTable)
				queryBuffer.AppendFormat(" AND ( [name] = @{0} )", commandContext.Command.AddParameter(columnAndTable.Column.PropertyName));

			// append the group clause
			queryBuffer.AppendFormat(" GROUP BY [{0}]", columnAndTable.Column.ColumnName);

			// add with custom mapping to the query
			commandContext.QueryBuilder.AddAdditionalQuery(queryBuffer.ToString(), (nodeset, reader) =>
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
			                                                                       	var result = FacetResult.Create(context, component.Facet, facetValues);

			                                                                       	// add the result to the facet set
			                                                                       	nodeset.AddFacet(result);
			                                                                       });
		}
		#endregion
	}
}