using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="long"/> to <see cref="int"/>.
	/// </summary>
	public class Int64ToInt32Converter : ConverterBase<Int64, Int32>
	{
		#region Overrides of ConverterBase<Int64,Int32>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Int32 DoConvert(IMansionContext context, Int64 source, Type sourceType)
		{
			return System.Convert.ToInt32(source, context.SystemCulture);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Int32 DoConvert(IMansionContext context, Int64 source, Type sourceType, Int32 defaultValue)
		{
			return System.Convert.ToInt32(source, context.SystemCulture);
		}
		#endregion
	}
}