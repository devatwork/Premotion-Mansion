using System;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="JObject"/> to <see cref="NodePointer"/>.
	/// </summary>
	public class JObjectToNodePointerConverter : ConverterBase<JObject, NodePointer>
	{
		#region Overrides of ConverterBase<JObject,NodePointer>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override NodePointer DoConvert(IMansionContext context, JObject source, Type sourceType)
		{
			return source.ToObject<NodePointer>();
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override NodePointer DoConvert(IMansionContext context, JObject source, Type sourceType, NodePointer defaultValue)
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