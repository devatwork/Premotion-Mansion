using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Combines boolean using the AND operator.
	/// </summary>
	[ScriptFunction("And")]
	public class And : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="one"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, bool one, bool other)
		{
			return one && other;
		}
	}
}