﻿using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptFunctions.Security
{
	/// <summary>
	/// Checks whether the user has permission to execute the specified operation.
	/// </summary>
	[ScriptFunction("HasPermission")]
	public class HasPermission : FunctionExpression
	{
		/// <summary>
		/// Checks whether the user has permission to execute the specified operation.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="resourceId">The ID of the protected resource.</param>
		/// <param name="operationId">The ID of the protected operation.</param>
		/// <returns>Returns true when the user has permission, otherwise false.</returns>
		public bool Evaluate(MansionContext context, string resourceId, string operationId)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// if the user is not authenticated, he/she will never have permission
			if (!context.CurrentUserState.IsAuthenticated)
				return false;

			// create the protected operation
			var operation = ProtectedOperation.Create(context, resourceId, operationId);

			// get the permission service
			var securityModelService = context.Nucleus.Get<ISecurityModelService>(context);

			// get the audit result
			var result = securityModelService.Audit(context, context.CurrentUserState, operation);

			// return the result
			return result.Granted;
		}
	}
}