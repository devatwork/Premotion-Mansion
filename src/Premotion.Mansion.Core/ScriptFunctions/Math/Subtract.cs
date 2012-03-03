using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Subtracts the second number from the first.
	/// </summary>
	[ScriptFunction("Subtract")]
	public class Subtract : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public int Evaluate(MansionContext context, int first, int second)
		{
			return first - second;
		}
	}
}