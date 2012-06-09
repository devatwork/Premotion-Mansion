using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="long"/>s.
	/// </summary>
	public class StringToInt64Converter : ConverterBase<String, Int64>
	{
		#region Overrides of ConverterBase<string,int>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override long DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return System.Convert.ToInt64(source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override long DoConvert(IMansionContext context, string source, Type sourceType, long defaultValue)
		{
			// try to convert
			try
			{
				return System.Convert.ToInt64(source);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}