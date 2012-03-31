using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether the value is empty.
	/// </summary>
	[ScriptFunction("IsEmpty")]
	public class IsEmpty : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, string input)
		{
			return string.IsNullOrEmpty(input);
		}
	}
}