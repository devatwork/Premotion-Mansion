using System;
using System.Text;
using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Specifications
{
	/// <summary>
	/// Specifies the SQL-server where query.
	/// </summary>
	public class SqlWhereSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs the SQL-server where specification using the given <paramref name="where"/>.
		/// </summary>
		/// <param name="where">The full-text query.</param>
		/// <exception cref="ArgumentNullException">Thrown</exception>
		public SqlWhereSpecification(string where)
		{
			// validate arguments
			if (string.IsNullOrEmpty(where))
				throw new ArgumentNullException("where");

			// set values
			Where = where;
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append("sql-where:").Append(Where);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the where clause.
		/// </summary>
		public string Where { get; private set; }
		#endregion
	}
}