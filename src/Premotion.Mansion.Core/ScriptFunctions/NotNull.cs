using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions
{
	/// <summary>
	/// Returns the first argument which is not null.
	/// </summary>
	[ScriptFunction("NotNull")]
	public class NotNull : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="candidateA"></param>
		/// <param name="candidateB"></param>
		/// <returns></returns>
		public object Evaluate(MansionContext context, object candidateA, object candidateB)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			if (candidateA != null)
				return candidateA;

			if (candidateB != null)
				return candidateB;

			return null;
		}
	}
}