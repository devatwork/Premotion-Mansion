using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Returns the first argument which is not empty.
	/// </summary>
	[ScriptFunction("NotEmpty")]
	public class NotEmpty : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="candidates"></param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, params string[] candidates)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (candidates == null)
				throw new ArgumentNullException("candidates");

			return candidates.Where(x => !string.IsNullOrEmpty(x)).FirstOrDefault();
		}
	}
}