using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Checks whether the user has permission to edit this site.
	/// </summary>
	[ScriptFunction("HasPortalAdminPermission")]
	public class HasPortalAdminPermission : FunctionExpression
	{
		/// <summary>
		/// Checks whether the user has permission to edit this site.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns true when the user has permission, otherwise false.</returns>
		public bool Evaluate(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if the backoffice user is authenticated
			if (!context.BackofficeUserState.IsAuthenticated)
				return false;

			// TODO: add permission checking here

			return true;
		}
	}
}