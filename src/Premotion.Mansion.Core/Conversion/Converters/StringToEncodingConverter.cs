using System;
using System.Text;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="Encoding"/>s.
	/// </summary>
	public class StringToEncodingConverter : ConverterBase<String, Encoding>
	{
		#region Overrides of ConverterBase<string,Encoding>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Encoding DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return Encoding.GetEncoding(source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Encoding DoConvert(IMansionContext context, string source, Type sourceType, Encoding defaultValue)
		{
			// try to convert
			try
			{
				return DoConvert(context, source, sourceType);
			}
			catch (ArgumentException)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}