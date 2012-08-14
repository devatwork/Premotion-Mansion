using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a column of a <see cref="Table"/>.
	/// </summary>
	public abstract class Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="columnName"></param>
		protected Column(string columnName) : this(columnName, columnName)
		{
		}
		/// <summary>
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="propertyName"></param>
		protected Column(string columnName, string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			ColumnName = columnName;
			PropertyName = propertyName;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this column.
		/// </summary>
		public string ColumnName { get; private set; }
		/// <summary>
		/// Gets the name of the property to which this column maps.
		/// </summary>
		public string PropertyName { get; private set; }
		#endregion
		#region Statement Mapping Methods
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void ToWhereStatement(IMansionContext context, QueryCommandContext commandContext, IEnumerable<object> values)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (values == null)
				throw new ArgumentNullException("values");
			if (commandContext == null)
				throw new ArgumentNullException("commandContext");

			// invoke template method
			DoToWhereStatement(context, values, commandContext);
		}
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		protected virtual void DoToWhereStatement(IMansionContext context, IEnumerable<object> values, QueryCommandContext commandContext)
		{
			throw new NotSupportedException();
		}
		#endregion
	}
}