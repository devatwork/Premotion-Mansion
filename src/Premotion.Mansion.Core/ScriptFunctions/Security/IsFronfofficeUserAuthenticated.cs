using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Security
{
	/// <summary>
	/// Checks whether the user is authenticated in the frontoffice.
	/// </summary>
	[ScriptFunction("IsFrontofficeUserAuthenticated")]
	public class IsFrontofficeUserAuthenticated : FunctionExpression
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
			return context.FrontofficeUserState.IsAuthenticated;
		}
	}
}