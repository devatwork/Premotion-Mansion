using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="float"/>s.
	/// </summary>
	public class StringToFloatConverter : ConverterBase<string, float>
	{
		#region Overrides of ConverterBase<string,float>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override float DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return System.Convert.ToSingle(source, context.SystemCulture);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override float DoConvert(IMansionContext context, string source, Type sourceType, float defaultValue)
		{
			// try to convert
			try
			{
				return System.Convert.ToSingle(source, context.SystemCulture);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}