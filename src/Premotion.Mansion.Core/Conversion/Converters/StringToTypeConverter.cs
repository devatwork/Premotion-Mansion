using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="string"/> to <see cref="Type"/>.
	/// </summary>
	public class StringToTypeConverter : ConverterBase<string, Type>
	{
		#region Overrides of ConverterBase<string,Type>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Type DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return Type.GetType(source, true, true);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Type DoConvert(IMansionContext context, string source, Type sourceType, Type defaultValue)
		{
			// try to convert
			try
			{
				return Type.GetType(source, true, true);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}