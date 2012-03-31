using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether the current item is the first item in the loop.
	/// </summary>
	[ScriptFunction("IsFirst")]
	public class IsFirst : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context)
		{
			return context.Stack.Peek<Loop>("Loop").IsFirst;
		}
	}
}