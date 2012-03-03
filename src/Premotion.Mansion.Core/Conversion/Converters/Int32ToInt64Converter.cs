using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="int"/> to <see cref="long"/>.
	/// </summary>
	public class Int32ToInt64Converter : ConverterBase<Int32, Int64>
	{
		#region Overrides of ConverterBase<Int32,Int64>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Int64 DoConvert(IContext context, Int32 source, Type sourceType)
		{
			return System.Convert.ToInt64(source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Int64 DoConvert(IContext context, Int32 source, Type sourceType, Int64 defaultValue)
		{
			return System.Convert.ToInt64(source);
		}
		#endregion
	}
}