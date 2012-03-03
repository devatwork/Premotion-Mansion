using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// This service manages <see cref="User"/>s, <see cref="UserGroup"/>s, <see cref="Role"/>s and <see cref="Permission"/>s.
	/// </summary>
	public interface ISecurityModelService : IService
	{
		#region User Group Methods
		/// <summary>
		/// Adds the <paramref name="user"/> to the <paramref name="group"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="user">The <see cref="User"/> which to add.</param>
		/// <param name="group">The <see cref="UserGroup"/> to which to add the user.</param>
		void AddGroupMembership(MansionContext context, User user, UserGroup group);
		/// <summary>
		/// Removes the <paramref name="user"/> from the <paramref name="group"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="user">The <see cref="User"/> which to remove.</param>
		/// <param name="group">The <see cref="UserGroup"/> from which to remove the user.</param>
		void RemoveGroupMembership(MansionContext context, User user, UserGroup group);
		/// <summary>
		/// Retrieves a <see cref="User"/> by it's ID.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/></param>
		/// <param name="currentUserState">The <see cref="UserState"/>.</param>
		/// <returns>Returns the loaded <see cref="User"/>.</returns>
		User RetrieveUser(MansionContext context, UserState currentUserState);
		#endregion
		#region Role Methods
		/// <summary>
		/// Adds the <paramref name="role"/> to the <paramref name="owner"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="owner">The <see cref="RoleOwner"/>.</param>
		/// <param name="role">The <see cref="Role"/> which to add to the <paramref name="owner"/>.</param>
		void AssignRole(MansionContext context, RoleOwner owner, Role role);
		/// <summary>
		/// Removes the <paramref name="role"/> from the <paramref name="owner"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="owner">The <see cref="RoleOwner"/>.</param>
		/// <param name="role">The <see cref="Role"/> which to remove from the <paramref name="owner"/>.</param>
		void RemoveRole(MansionContext context, RoleOwner owner, Role role);
		/// <summary>
		/// Retrieves the assigned <see cref="Role"/> IDs of the specified <paramref name="roleOwner"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="roleOwner">The <see cref="RoleOwner"/> from which to get the assigned <see cref="Role"/> IDs.</param>
		/// <returns>Returns the assigned <see cref="Role"/> IDS.</returns>
		string[] RetrieveAssignedRoleIds(MansionContext context, RoleOwner roleOwner);
		#endregion
		#region Permission Methods
		/// <summary>
		/// Adds <see cref="Permission"/> for the <paramref name="operation"/> to the <paramref name="role"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="role">The <see cref="Role"/> to which to add the <see cref="Permission"/>.</param>
		/// <param name="operation">The <see cref="ProtectedOperation"/> given <see cref="Permission"/> to.</param>
		/// <param name="granted">Flag indicating whether the <see cref="Permission"/> is granted or not.</param>
		/// <param name="priority">The priority of this <see cref="Permission"/>. <see cref="Permission"/>s with higher <see cref="Permission.Priority"/> will overrule those with lower <see cref="Permission.Priority"/>.</param>
		void AssignPermission(MansionContext context, Role role, ProtectedOperation operation, bool granted = true, int priority = 5);
		/// <summary>
		/// Removes <see cref="Permission"/> for the <paramref name="operation"/> from the <paramref name="role"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="role">The <see cref="Role"/> from which remove add the <see cref="Permission"/>.</param>
		/// <param name="operation">The <see cref="ProtectedOperation"/> from which to revoke <see cref="Permission"/>.</param>
		void RemovePermission(MansionContext context, Role role, ProtectedOperation operation);
		/// <summary>
		/// Checks whether the <paramref name="userState"/> has permission to <paramref name="operation"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="userState">The <see cref="User"/> which to check.</param>
		/// <param name="operation">The <see cref="ProtectedOperation"/> on which to check.</param>
		/// <returns>Returns the <see cref="AuditResult"/>.</returns>
		AuditResult Audit(MansionContext context, UserState userState, ProtectedOperation operation);
		#endregion
	}
}