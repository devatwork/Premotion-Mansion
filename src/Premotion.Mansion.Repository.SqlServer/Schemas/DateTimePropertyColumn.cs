﻿using System;
using System.Data.SqlTypes;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represens a <see cref="DateTime"/> property column.
	/// </summary>
	public class DateTimePropertyColumn : PropertyColumn
	{
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="columnName">The name of the column.</param>
		private DateTimePropertyColumn(string propertyName, string columnName) : base(propertyName, columnName)
		{
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates a <see cref="DateTimePropertyColumn"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="columnName">The name of the column.</param>
		/// <param name="properties">The properties of the column.</param>
		/// <returns>Returns the created column.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static DateTimePropertyColumn CreateDateTimeColumn(IMansionContext context, string propertyName, string columnName, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// create the column
			var column = new DateTimePropertyColumn(propertyName, columnName);

			// init the column from the properties
			Initialize(context, column, properties);

			// return the created column
			return column;
		}
		#endregion
		#region Overrides of PropertyColumn
		/// <summary>
		/// Normalizes the value of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to normalize.</param>
		/// <returns>Returns the normalized value.</returns>
		protected override object Normalize(IMansionContext context, object input)
		{
			// parse to datetime
			var conversionService = context.Nucleus.ResolveSingle<IConversionService>();
			var dateTime = conversionService.Convert<DateTime>(context, base.Normalize(context, input));
			return (dateTime >= SqlDateTime.MinValue.Value) ? (object) dateTime : null;
		}
		#endregion
	}
}