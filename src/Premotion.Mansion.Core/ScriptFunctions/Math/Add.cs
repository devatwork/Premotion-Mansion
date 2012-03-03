using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Adds the second number to the first.
	/// </summary>
	[ScriptFunction("Add")]
	public class Add : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="first"></param>
		/// <returns></returns>
		public int Evaluate(MansionContext context, int first)
		{
			return Evaluate(context, first, 1);
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public int Evaluate(MansionContext context, int first, int second)
		{
			return first + second;
		}
	}
}