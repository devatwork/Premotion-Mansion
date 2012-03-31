using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="Uri"/>s.
	/// </summary>
	public class StringToUriConverter : ConverterBase<String, Uri>
	{
		#region Overrides of ConverterBase<string,Uri>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Uri DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return new Uri((source ?? string.Empty), UriKind.RelativeOrAbsolute);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Uri DoConvert(IMansionContext context, string source, Type sourceType, Uri defaultValue)
		{
			// try to convert
			try
			{
				return new Uri((source ?? string.Empty), UriKind.RelativeOrAbsolute);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}