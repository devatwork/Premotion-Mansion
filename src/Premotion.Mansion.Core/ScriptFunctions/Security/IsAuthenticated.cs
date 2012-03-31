using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Security
{
	/// <summary>
	/// Checks whether an user is authenticated for the current request context or not.
	/// </summary>
	[ScriptFunction("IsAuthenticated")]
	public class IsAuthenticated : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if the user is authenticated for the current request
			return context.CurrentUserState.IsAuthenticated;
		}
	}
}