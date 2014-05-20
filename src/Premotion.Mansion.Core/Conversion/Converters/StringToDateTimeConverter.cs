using System;
using System.Globalization;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="string"/> to <see cref="DateTime"/>.
	/// </summary>
	public class StringToDateTimeConverter : ConverterBase<string, DateTime>
	{
		#region Constants
		private static readonly string[] Formats = new[] {"r", "U", "f", "F", "d", "D", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy-MM-dd", "yyyy/MM/dd HH:mm:ss", "yyyy/MM/dd HH:mm", "yyyy/MM/dd", "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm", "dd-MM-yyyy", "d MMMM yyyy HH:mm:ss", "d MMMM yyyy HH:mm", "d MMMM yyyy"};
		#endregion
		#region Overrides of ConverterBase<string,DateTime>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override DateTime DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return DoConvert(context, source, sourceType, DateTime.MinValue);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override DateTime DoConvert(IMansionContext context, string source, Type sourceType, DateTime defaultValue)
		{
			// check input
			if (string.IsNullOrEmpty(source))
				return defaultValue;

			// try to parse the date with any of the formats
			DateTime result;
			if (DateTime.TryParseExact(source, Formats, context.UserInterfaceCulture, DateTimeStyles.None, out result))
				return result;

			return defaultValue;
		}
		#endregion
	}
}