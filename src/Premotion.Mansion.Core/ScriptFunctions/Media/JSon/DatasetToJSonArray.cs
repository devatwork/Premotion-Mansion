using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Media.JSon
{
	/// <summary>
	/// Renders the dataset as a JSon array.
	/// </summary>
	[ScriptFunction("DatasetToJSonArray")]
	public class DatasetToJSonArray : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dataset"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, Dataset dataset)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (dataset == null)
				throw new ArgumentNullException("dataset");

			// write the dataspace out to JSon array
			var buffer = new StringBuilder();
			using (var textWriter = new StringWriter(buffer))
			using (var jsonWriter = new JsonTextWriter(textWriter))
				dataset.WriteAsJSonArray(jsonWriter);

			// return the content of the buffer.
			return buffer.ToString();
		}
	}
}