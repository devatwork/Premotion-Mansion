using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether the value is null.
	/// </summary>
	[ScriptFunction("IsNull")]
	public class IsNull : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, object input)
		{
			return input == null;
		}
	}
}