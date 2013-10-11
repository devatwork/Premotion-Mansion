using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Media.JSon
{
	/// <summary>
	/// Renders the datapsace as a json object.
	/// </summary>
	[ScriptFunction("ToJSon")]
	public class ToJSon : FunctionExpression
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
				// if the dataspace is a dataset, write it as a array, otherwise write it as an object
				var dataset = dataspace as Dataset;
				if (dataset != null)
					dataset.WriteAsJSonArray(jsonWriter);
				else
					dataspace.WriteAsJSonObject(jsonWriter);
			}

			// return the content of the buffer.
			return buffer.ToString();
		}
	}
}