using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="long"/> to <see cref="double"/>.
	/// </summary>
	public class Int64ToDoubleConverter : ConverterBase<Int64, double>
	{
		#region Overrides of ConverterBase<Int64,Int32>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override double DoConvert(IMansionContext context, Int64 source, Type sourceType)
		{
			return source;
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override double DoConvert(IMansionContext context, Int64 source, Type sourceType, double defaultValue)
		{
			return source;
		}
		#endregion
	}
}