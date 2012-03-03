using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Negates the expression.
	/// </summary>
	[ScriptFunction("Not")]
	public class Not : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="condition"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, bool condition)
		{
			return !condition;
		}
	}
}