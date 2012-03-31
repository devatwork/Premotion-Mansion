using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="bool"/>s.
	/// </summary>
	public class StringToBooleanConverter : ConverterBase<string, bool>
	{
		#region Overrides of ConverterBase<string,bool>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override bool DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return Convert(source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override bool DoConvert(IMansionContext context, string source, Type sourceType, bool defaultValue)
		{
			// try to convert
			try
			{
				return Convert(source);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		/// <summary>
		/// Makes the actual conversion.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		private static bool Convert(string source)
		{
			if (string.IsNullOrEmpty(source))
				return false;

			switch (source.ToLower())
			{
				case "true":
				case "on":
				case "yes":
				case "1":
				{
					return true;
				}
				case "false":
				case "off":
				case "no":
				case "0":
				{
					return false;
				}
			}

			// try to convert
			return System.Convert.ToBoolean(source);
		}
		#endregion
	}
}