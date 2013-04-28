using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Rounds a double-precision floating-point value to the nearest integral value.
	/// </summary>
	[ScriptFunction("Round")]
	public class Round : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Evaluate(IMansionContext context, double value)
		{
			return (int) System.Math.Round(value);
		}
	}
}