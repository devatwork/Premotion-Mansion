using System;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="string"/> to <see cref="NodePointer"/>.
	/// </summary>
	public class StringToNodePointerConverter : ConverterBase<string, NodePointer>
	{
		#region Overrides of ConverterBase<string,NodePointer>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override NodePointer DoConvert(IMansionContext context, string source, Type sourceType)
		{
			return NodePointer.Parse(source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override NodePointer DoConvert(IMansionContext context, string source, Type sourceType, NodePointer defaultValue)
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