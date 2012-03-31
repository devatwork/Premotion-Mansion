using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from an array of <see cref="byte"/>s to <see cref="IPropertyBag"/>.
	/// </summary>
	public class ByteArrayToPropertyBagConverter : ConverterBase<byte[], IPropertyBag>
	{
		#region Overrides of ConverterBase<byte[],IPropertyBag>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override IPropertyBag DoConvert(IMansionContext context, byte[] source, Type sourceType)
		{
			// deserialize
			using (var bufferStream = new MemoryStream(source))
			using (var reader = new BsonReader(bufferStream))
				return serializer.Deserialize<PropertyBag>(reader);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override IPropertyBag DoConvert(IMansionContext context, byte[] source, Type sourceType, IPropertyBag defaultValue)
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