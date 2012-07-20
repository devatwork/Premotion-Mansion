using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Returns the smaller of two double-precision floating-point numbers.
	/// </summary>
	[ScriptFunction("Min")]
	public class Min : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public double Evaluate(IMansionContext context, double first, double second)
		{
			return System.Math.Min(first, second);
		}
	}
}