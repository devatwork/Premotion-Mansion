using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="int"/> to <see cref="double"/>.
	/// </summary>
	public class Int32ToIntDoubleConverter : ConverterBase<Int32, double>
	{
		#region Overrides of ConverterBase<Int32,double>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override double DoConvert(IMansionContext context, Int32 source, Type sourceType)
		{
			return System.Convert.ToDouble(source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override double DoConvert(IMansionContext context, Int32 source, Type sourceType, double defaultValue)
		{
			return System.Convert.ToDouble(source);
		}
		#endregion
	}
}