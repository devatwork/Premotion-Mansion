using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Combines boolean using the OR operator.
	/// </summary>
	[ScriptFunction("Or")]
	public class Or : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="candidates"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, params bool[] candidates)
		{
			return candidates.Any(candidate => candidate);
		}
	}
}