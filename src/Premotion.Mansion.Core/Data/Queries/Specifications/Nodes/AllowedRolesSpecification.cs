using System;
using System.Text;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.Data.Queries.Specifications.Nodes
{
	/// <summary>
	/// Specifies the security of the current specification.
	/// </summary>
	public class AllowedRolesSpecification : Specification
	{
		#region Constants
		private static readonly AllowedRolesSpecification AnyInstance = new AllowedRolesSpecification(new string[] {});
		#endregion
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="allowedRoleIds"></param>
		private AllowedRolesSpecification(string[] allowedRoleIds)
		{
			// validate arguments
			if (allowedRoleIds == null)
				throw new ArgumentNullException("allowedRoleIds");

			// set values
			RoleIds = allowedRoleIds;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs an <see cref="AllowedRolesSpecification"/> filter which accepts any roles.
		/// </summary>
		/// <returns>Returns the created specification.</returns>
		public static AllowedRolesSpecification Any()
		{
			return AnyInstance;
		}
		/// <summary>
		/// Creates a specification which filters based on the roles of the currently authenticated user.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the created specification.</returns>
		public static AllowedRolesSpecification UserRoles(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the security service
			var securityModelService = context.Nucleus.ResolveSingle<ISecurityModelService>();

			// retrieve the user
			var user = securityModelService.RetrieveUser(context, context.CurrentUserState);

			// use the default permissions of the user
			var roleIds = securityModelService.RetrieveAssignedRoleIds(context, user);

			// return the allow roles specification
			return new AllowedRolesSpecification(roleIds);
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append("allowed-role-ids:").Append(RoleIds.Length == 0 ? "any" : string.Join(",", RoleIds));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the IDs of the roles.
		/// </summary>
		public string[] RoleIds { get; private set; }
		#endregion
	}
}