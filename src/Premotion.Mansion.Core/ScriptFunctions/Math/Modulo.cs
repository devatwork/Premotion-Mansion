using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Computes the remainder of division of the first- by the second number.
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