using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Modulo computes a remainder. Multiplies the first value with the second value.
	/// </summary>
	[ScriptFunction("Modulo")]
	public class Modulo : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public double Evaluate(IMansionContext context, double first, double second)
		{
			return first % second;
		}
	}
}