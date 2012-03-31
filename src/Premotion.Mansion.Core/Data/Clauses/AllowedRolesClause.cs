using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// Specifies the security of the current query.
	/// </summary>
	public class AllowedRolesClause : NodeQueryClause
	{
		#region Nested type: AllowedRolesClauseInterpreter
		/// <summary>
		/// Interprets <see cref = "AllowedRolesClause" />.
		/// </summary>
		public class AllowedRolesClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public AllowedRolesClauseInterpreter() : base(10)
			{
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// Interprets the input.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext" />.</param>
			/// <param name="input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(IMansionContext context, IPropertyBag input)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (input == null)
					throw new ArgumentNullException("input");

				// if the authorization is bypassed do not add the security check
				bool bypassAuthorization;
				if (input.TryGetAndRemove(context, "bypassAuthorization", out bypassAuthorization) && bypassAuthorization)
					yield break;

				// get the security model service
				var securityModelService = context.Nucleus.ResolveSingle<ISecurityModelService>();

				// retrieve the user
				var user = securityModelService.RetrieveUser(context, context.CurrentUserState);

				// use the default permissions of the user
				var roleIds = securityModelService.RetrieveAssignedRoleIds(context, user);

				// create and return the clause
				yield return new AllowedRolesClause(roleIds);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="allowedRoleIds"></param>
		private AllowedRolesClause(string[] allowedRoleIds)
		{
			// validate arguments
			if (allowedRoleIds == null)
				throw new ArgumentNullException("allowedRoleIds");

			// set values
			RoleIds = allowedRoleIds;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the IDs of the roles.
		/// </summary>
		public string[] RoleIds { get; private set; }
		#endregion
		#region Overrides of NodeQueryClause
		/// <summary>
		/// Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("allowed-role-ids:{0}", string.Join(",", RoleIds));
		}
		#endregion
	}
}