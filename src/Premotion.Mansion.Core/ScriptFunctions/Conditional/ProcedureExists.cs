using System;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether a procedure exists or not.
	/// </summary>
	[ScriptFunction("procedureExists")]
	public class ProcedureExists : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="procedureName"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, string procedureName)
		{
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (procedureName == null)
				throw new ArgumentNullException("procedureName");

			IScript script;
			return context.ProcedureStack.TryPeek(procedureName, out script);
		}
	}
}