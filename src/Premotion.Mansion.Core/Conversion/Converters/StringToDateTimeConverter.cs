﻿using System;
using System.Globalization;
using System.Linq;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="string"/> to <see cref="DateTime"/>.
	/// </summary>
	public class StringToDateTimeConverter : ConverterBase<string, DateTime>
	{
		#region Constants
		private static readonly string[] formats = new[] {"r", "U", "f", "F", "d", "D", "yyyy-MM-dd", "yyyy/MM/dd", "dd-MM-yyyy", "d MMMM yyyy"};
		#endregion
		#region Overrides of ConverterBase<string,DateTime>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override DateTime DoConvert(IContext context, string source, Type sourceType)
		{
			return DoConvert(context, source, sourceType, DateTime.MinValue);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override DateTime DoConvert(IContext context, string source, Type sourceType, DateTime defaultValue)
		{
			// check input
			if (string.IsNullOrEmpty(source))
				return defaultValue;

			// try to parse the date with any of the formats
			var result = defaultValue;
			if (formats.Any(format => DateTime.TryParseExact(source, format, context.Cast<MansionContext>().UserInterfaceCulture, DateTimeStyles.None, out result)))
				return result;

			return defaultValue;
		}
		#endregion
	}
}