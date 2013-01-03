using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="float"/> to <see cref="double"/>.
	/// </summary>
	public class FloatToDoubleConverter : ConverterBase<float, double>
	{
		#region Overrides of ConverterBase<Int64,Int32>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override double DoConvert(IMansionContext context, float source, Type sourceType)
		{
			return System.Convert.ToDouble(source, context.SystemCulture);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override double DoConvert(IMansionContext context, float source, Type sourceType, double defaultValue)
		{
			return System.Convert.ToDouble(source, context.SystemCulture);
		}
		#endregion
	}
}