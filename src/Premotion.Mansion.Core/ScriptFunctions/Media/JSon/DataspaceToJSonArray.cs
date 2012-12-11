using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Media.JSon
{
	/// <summary>
	/// Renders the datapsace as a JSon array.
	/// </summary>
	[ScriptFunction("DataspaceToJSonArray")]
	public class DataspaceToJSonArray : FunctionExpression
	{
		private static readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings());
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dataspace"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, IPropertyBag dataspace)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (dataspace == null)
				throw new ArgumentNullException("dataspace");

			// write the dataspace out to JSon array
			var buffer = new StringBuilder();
			using (var textWriter = new StringWriter(buffer))
			using (var jsonWriter = new JsonTextWriter(textWriter))
			{
				jsonWriter.WriteStartArray();

				// loop over the values in the dataspace
				foreach (var property in dataspace.Where(x => x.Value != null))
				{
					// get the type
					var propertyType = property.Value.GetType();

					// if this type has no serializer, convert it to string first
					var value = property.Value;
					if (!Serializer.Converters.Any(candidate => candidate.CanConvert(propertyType)))
						value = dataspace.Get<string>(context, property.Key);

					// write the value
					jsonWriter.WriteValue(value);
				}

				jsonWriter.WriteEndArray();
			}

			// return the content of the buffer.
			return buffer.ToString();
		}
	}
}