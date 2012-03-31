using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether a node exists.
	/// </summary>
	[ScriptFunction("IsNode")]
	public class IsNode : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, Node node)
		{
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return node != null;
		}
	}
}