using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="char"/>s.
	/// </summary>
	public class StringToCharConverter : ConverterBase<string, char>
	{
		#region Overrides of ConverterBase<string,Char>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override char DoConvert(IMansionContext context, string source, Type sourceType)
		{
			// check length
			if (source.Length != 1)
				throw new InvalidOperationException(string.Format("String '{0}' does not contain one charachter.", source));

			return source[0];
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override char DoConvert(IMansionContext context, string source, Type sourceType, char defaultValue)
		{
			// try to convert
			try
			{
				return DoConvert(context, source, sourceType);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}