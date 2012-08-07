using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Converts <see cref="IsPropertyInSpecification"/> to a statement.
	/// </summary>
	public class IsPropertyInSpecificationConverter : SpecificationConverter<IsPropertyInSpecification>
	{
		#region Overrides of SpecificationConverter<IsPropertyEqualSpecification>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, IsPropertyInSpecification specification, QueryCommandContext commandContext)
		{
			// get the table in which the column exists from the schema
			var pair = commandContext.Schema.FindTableAndColumn(specification.PropertyName);

			// allow the table to map the value
			pair.Table.ToWhereStatement(context, pair.Column, specification.Values, commandContext);
		}
		#endregion
	}
}