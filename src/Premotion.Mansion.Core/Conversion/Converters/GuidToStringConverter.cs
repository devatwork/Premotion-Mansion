using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="Guid"/> to <see cref="string"/>.
	/// </summary>
	public class GuidToStringConverter : ConverterBase<Guid, string>
	{
		#region Overrides of ConverterBase<string,Guid>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IMansionContext context, Guid source, Type sourceType)
		{
			return source.ToString();
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IMansionContext context, Guid source, Type sourceType, string defaultValue)
		{
			return source.ToString();
		}
		#endregion
	}
}