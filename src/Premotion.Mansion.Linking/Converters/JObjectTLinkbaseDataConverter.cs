using System;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Linking.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="JObject"/> to <see cref="LinkbaseData"/>.
	/// </summary>
	public class JObjectTLinkbaseDataConverter : ConverterBase<JObject, LinkbaseData>
	{
		#region Overrides of ConverterBase<JObject,NodePointer>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override LinkbaseData DoConvert(IMansionContext context, JObject source, Type sourceType)
		{
			return source.ToObject<LinkbaseData>();
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override LinkbaseData DoConvert(IMansionContext context, JObject source, Type sourceType, LinkbaseData defaultValue)
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