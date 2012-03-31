using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.JSon
{
	/// <summary>
	/// Renders the datapsace as a JSon array.
	/// </summary>
	[ScriptFunction("DataspaceToJSonArray")]
	public class DataspaceToJSonArray : FunctionExpression
	{
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
				foreach (var property in dataspace)
					jsonWriter.WriteValue(property.Value);

				jsonWriter.WriteEndArray();
			}

			// return the content of the buffer.
			return buffer.ToString();
		}
	}
}