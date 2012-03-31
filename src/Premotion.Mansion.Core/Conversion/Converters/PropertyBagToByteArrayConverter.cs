using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="IPropertyBag"/> to an array of <see cref="byte"/>s.
	/// </summary>
	public class PropertyBagToByteArrayConverter : ConverterBase<IPropertyBag, byte[]>
	{
		#region Overrides of ConverterBase<IPropertyBag,byte[]>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override byte[] DoConvert(IMansionContext context, IPropertyBag source, Type sourceType)
		{
			using (var bufferStream = new MemoryStream())
			using (var writer = new BsonWriter(bufferStream))
			{
				serializer.Serialize(writer, source);
				return bufferStream.GetBuffer();
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
		protected override byte[] DoConvert(IMansionContext context, IPropertyBag source, Type sourceType, byte[] defaultValue)
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