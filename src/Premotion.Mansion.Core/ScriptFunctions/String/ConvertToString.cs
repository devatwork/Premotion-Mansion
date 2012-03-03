using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Converts the object to a string.
	/// </summary>
	[ScriptFunction("ConvertToString")]
	public class ConvertToString : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, object input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				return string.Empty;

			// get the conversion service
			var conversionService = context.Nucleus.Get<IConversionService>(context);

			// convert the object to string
			return conversionService.Convert(context, input, string.Empty);
		}
	}
}