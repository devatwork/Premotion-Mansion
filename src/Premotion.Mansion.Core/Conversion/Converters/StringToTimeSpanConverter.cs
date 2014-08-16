using System;
using System.Globalization;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="string"/> to <see cref="TimeSpan"/>.
	/// </summary>
	public class StringToTimeSpanConverter : ConverterBase<string, TimeSpan>
	{
		#region Constants
		private static readonly string[] Formats = new[] { "h\\:m", "h\\:mm", "hh\\:m", "hh\\:mm", "hh\\:mm\\:s", "hh\\:mm\\:ss" };
		#endregion
		#region Overrides of ConverterBase<string,TimeSpan>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override TimeSpan DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return DoConvert(context, source, sourceType, TimeSpan.MinValue);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override TimeSpan DoConvert(IMansionContext context, string source, Type sourceType, TimeSpan defaultValue)
		{
			// check input
			if (string.IsNullOrEmpty(source))
				return defaultValue;

			// try to parse the time with any of the formats
			TimeSpan result;
			if (TimeSpan.TryParseExact(source, Formats, context.UserInterfaceCulture, TimeSpanStyles.None, out result))
				return result;

			return defaultValue;
		}
		#endregion
	}
}