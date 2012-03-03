using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="int"/> to <see cref="bool"/>.
	/// </summary>
	public class Int32ToBooleanConverter : ConverterBase<Int32, bool>
	{
		#region Overrides of ConverterBase<int,bool>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override bool DoConvert(IContext context, int source, Type sourceType)
		{
			return (source) > 0;
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override bool DoConvert(IContext context, int source, Type sourceType, bool defaultValue)
		{
			return (source) > 0;
		}
		#endregion
	}
}