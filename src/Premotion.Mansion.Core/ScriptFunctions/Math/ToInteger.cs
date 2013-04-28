using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Converts the input to an integer.
	/// </summary>
	[ScriptFunction("ToInteger")]
	public class ToInteger : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Evaluate(IMansionContext context, int input)
		{
			return input;
		}
	}
}