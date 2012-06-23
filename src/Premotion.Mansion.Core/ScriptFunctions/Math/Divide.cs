using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Multiplies the first value with the second value.
	/// </summary>
	[ScriptFunction("Divide")]
	public class Divide : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public double Evaluate(IMansionContext context, double first, double second)
		{
			return first/second;
		}
	}
}