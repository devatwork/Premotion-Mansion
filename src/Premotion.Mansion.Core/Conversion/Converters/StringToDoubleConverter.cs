using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="double"/>s.
	/// </summary>
	public class StringToDoubleConverter : ConverterBase<String, double>
	{
		#region Overrides of ConverterBase<string,double>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override double DoConvert(IContext context, string source, Type sourceType)
		{
			source = source.Trim();
			return string.IsNullOrEmpty(source) ? 0 : System.Convert.ToDouble(source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override double DoConvert(IContext context, string source, Type sourceType, double defaultValue)
		{
			// try to convert
			try
			{
				return System.Convert.ToDouble(source.Trim());
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}