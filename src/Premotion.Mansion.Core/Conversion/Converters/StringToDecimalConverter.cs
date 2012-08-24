using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="decimal"/>s.
	/// </summary>
	public class StringToDecimalConverter : ConverterBase<String, decimal>
	{
		#region Overrides of ConverterBase<string,int>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override decimal DoConvert(IMansionContext context, string source, Type sourceType)
		{
			source = source.Trim();
			return string.IsNullOrEmpty(source) ? 0 : System.Convert.ToDecimal(source, context.SystemCulture);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override decimal DoConvert(IMansionContext context, string source, Type sourceType, decimal defaultValue)
		{
			// try to convert
			try
			{
				return System.Convert.ToDecimal(source.Trim(), context.SystemCulture);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}