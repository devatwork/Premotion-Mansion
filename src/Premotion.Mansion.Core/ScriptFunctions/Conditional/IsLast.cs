using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether the current item is the last item in the loop.
	/// </summary>
	[ScriptFunction("IsLast")]
	public class IsLast : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context)
		{
			return context.Stack.Peek<Loop>("Loop").IsLast;
		}
	}
}