using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="string"/> to <see cref="Guid"/>.
	/// </summary>
	public class StringToGuidConverter : ConverterBase<string, Guid>
	{
		#region Overrides of ConverterBase<string,Guid>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Guid DoConvert(IContext context, string source, Type sourceType)
		{
			Guid result;
			if (!Guid.TryParse(source, out result))
				result = Guid.Empty;
			return result;
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Guid DoConvert(IContext context, string source, Type sourceType, Guid defaultValue)
		{
			Guid guid;
			if (Guid.TryParse(source, out guid))
				return guid;
			return defaultValue;
		}
		#endregion
	}
}