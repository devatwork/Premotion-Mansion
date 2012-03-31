using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using SystemConverter = System.Convert;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="IPropertyBag"/> to <see cref="string"/>.
	/// </summary>
	public class PropertyBagToBase64Converter : ConverterBase<IPropertyBag, string>
	{
		#region Overrides of ConverterBase<IPropertyBag,string>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IMansionContext context, IPropertyBag source, Type sourceType)
		{
			using (var bufferStream = new MemoryStream())
			using (var writer = new BsonWriter(bufferStream))
			{
				serializer.Serialize(writer, source);
				return SystemConverter.ToBase64String(bufferStream.GetBuffer());
			}
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IMansionContext context, IPropertyBag source, Type sourceType, string defaultValue)
		{
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
		#region Private Fields
		private static readonly JsonSerializer serializer = new JsonSerializer();
		#endregion
	}
}