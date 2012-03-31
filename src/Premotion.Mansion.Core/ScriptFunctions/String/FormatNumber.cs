using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Implements the format number function.
	/// </summary>
	[ScriptFunction("FormatNumber")]
	public class FormatNumber : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="number"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, double number, string format)
		{
			// validate arguments
			if (double.IsNaN(number))
				return string.Empty;

			// return the formated date
			return number.ToString(format, context.UserInterfaceCulture);
		}
	}
}