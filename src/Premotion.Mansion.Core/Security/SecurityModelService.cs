using System;
using System.Linq;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Provides implementation for <see cref="ISecurityModelService"/>.
	/// </summary>
	public class SecurityModelService : ISecurityModelService
	{
		#region Constructors
		/// <summary>
		/// Constructs the security model service.
		/// </summary>
		/// <param name="persistenceService">The <see cref="ISecurityPersistenceService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="persistenceService"/> is null.</exception>
		public SecurityModelService(ISecurityPersistenceService persistenceService)
		{
			// validate arguments
			if (persistenceService == null)
				throw new ArgumentNullException("persistenceService");

			// set values
			this.persistenceService = persistenceService;
		}
		#endregion
		#region Implementation of ISecurityModelService
		/// <summary>
		/// Adds the <paramref name="user"/> to the <paramref name="group"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="user">The <see cref="User"/> which to add.</param>
		/// <param name="group">The <see cref="UserGroup"/> to which to add the user.</param>
		public void AddGroupMembership(IMansionContext context, User user, UserGroup group)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (user == null)
				throw new ArgumentNullException("user");
			if (group == null)
				throw new ArgumentNullException("group");

			// store in perstistence
			PersistenceService.AddGroupMembership(context, user, group);

			// add all the roles of the group to the user
			foreach (var groupRole in group.Roles)
				user.Add(groupRole);
		}
		/// <summary>
		/// Removes the <paramref name="user"/> from the <paramref name="group"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="user">The <see cref="User"/> which to remove.</param>
		/// <param name="group">The <see cref="UserGroup"/> from which to remove the user.</param>
		public void RemoveGroupMembership(IMansionContext context, User user, UserGroup group)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (user == null)
				throw new ArgumentNullException("user");
			if (group == null)
				throw new ArgumentNullException("group");

			// store in perstistence
			PersistenceService.RemoveGroupMembership(context, user, group);

			// remove all the roles of the group from the user
			foreach (var groupRole in group.Roles)
				user.Remove(groupRole);
		}
		/// <summary>
		/// Retrieves a <see cref="User"/> by it's ID.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/></param>
		/// <param name="currentUserState">The <see cref="UserState"/>.</param>
		/// <returns>Returns the loaded <see cref="User"/>.</returns>
		public User RetrieveUser(IMansionContext context, UserState currentUserState)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (currentUserState == null)
				throw new ArgumentNullException("currentUserState");

			return persistenceService.RetrieveUser(context, currentUserState.Id);
		}
		/// <summary>
		/// Adds the <paramref name="role"/> to the <paramref name="owner"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="owner">The <see cref="RoleOwner"/>.</param>
		/// <param name="role">The <see cref="Role"/> which to add to the <paramref name="owner"/>.</param>
		public void AssignRole(IMansionContext context, RoleOwner owner, Role role)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (owner == null)
				throw new ArgumentNullException("owner");
			if (role == null)
				throw new ArgumentNullException("role");

			// store in perstistence
			PersistenceService.AssignRole(context, owner, role);

			// add the role
			owner.Add(role);
		}
		/// <summary>
		/// Removes the <paramref name="role"/> from the <paramref name="owner"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="owner">The <see cref="RoleOwner"/>.</param>
		/// <param name="role">The <see cref="Role"/> which to remove from the <paramref name="owner"/>.</param>
		public void RemoveRole(IMansionContext context, RoleOwner owner, Role role)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (owner == null)
				throw new ArgumentNullException("owner");
			if (role == null)
				throw new ArgumentNullException("role");

			// store in perstistence
			PersistenceService.RemoveRole(context, owner, role);

			// add the role
			owner.Remove(role);
		}
		/// <summary>
		/// Retrieves the assigned <see cref="Role"/> IDs of the specified <paramref name="roleOwner"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="roleOwner">The <see cref="RoleOwner"/> from which to get the assigned <see cref="Role"/> IDs.</param>
		/// <returns>Returns the assigned <see cref="Role"/> IDS.</returns>
		public string[] RetrieveAssignedRoleIds(IMansionContext context, RoleOwner roleOwner)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (roleOwner == null)
				throw new ArgumentNullException("roleOwner");

			// extract the IDs
			return roleOwner.Roles.Select(x => x.Id).ToArray();
		}
		/// <summary>
		/// Adds <see cref="Permission"/> for the <paramref name="operation"/> to the <paramref name="role"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="role">The <see cref="Role"/> to which to add the <see cref="Permission"/>.</param>
		/// <param name="operation">The <see cref="ProtectedOperation"/> given <see cref="Permission"/> to.</param>
		/// <param name="granted">Flag indicating whether the <see cref="Permission"/> is granted or not.</param>
		/// <param name="priority">The priority of this <see cref="Permission"/>. <see cref="Permission"/>s with higher <see cref="Permission.Priority"/> will overrule those with lower <see cref="Permission.Priority"/>.</param>
		public void AssignPermission(IMansionContext context, Role role, ProtectedOperation operation, bool granted = true, int priority = 5)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (role == null)
				throw new ArgumentNullException("role");
			if (operation == null)
				throw new ArgumentNullException("operation");

			// check if the permission should be updated or added to the role
			Permission permission;
			if (role.TryGet(operation, out permission))
			{
				// update the existing permission
				permission.Granted = granted;
				permission.Priority = priority;

				// store in persistence
				PersistenceService.UpdatePermission(context, permission);
			}
			else
			{
				// add a new permission
				permission = new Permission
				             {
				             	Granted = granted,
				             	Operation = operation,
				             	Priority = priority
				             };

				// store in persistence
				PersistenceService.CreatePermission(context, role, permission);

				// add the permission to the role
				role.Add(permission);
			}
		}
		/// <summary>
		/// Removes <see cref="Permission"/> for the <paramref name="operation"/> from the <paramref name="role"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="role">The <see cref="Role"/> from which remove add the <see cref="Permission"/>.</param>
		/// <param name="operation">The <see cref="ProtectedOperation"/> from which to revoke <see cref="Permission"/>.</param>
		public void RemovePermission(IMansionContext context, Role role, ProtectedOperation operation)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (role == null)
				throw new ArgumentNullException("role");
			if (operation == null)
				throw new ArgumentNullException("operation");

			// if there is no permission set ignore the call
			Permission permission;
			if (!role.TryGet(operation, out permission))
				return;

			// store in persistence
			PersistenceService.DeletePermission(context, role, permission);

			// remove the permission
			role.Remove(permission);
		}
		/// <summary>
		/// Checks whether the <paramref name="userState"/> has permission to <paramref name="operation"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="userState">The <see cref="User"/> which to check.</param>
		/// <param name="operation">The <see cref="ProtectedOperation"/> on which to check.</param>
		/// <returns>Returns the <see cref="AuditResult"/>.</returns>
		public AuditResult Audit(IMansionContext context, UserState userState, ProtectedOperation operation)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (userState == null)
				throw new ArgumentNullException("userState");
			if (operation == null)
				throw new ArgumentNullException("operation");

			// get the user from the state
			var user = PersistenceService.RetrieveUser(context, userState.Id);

			// find all the permissions for the operation in all roles
			var permissions = user.Roles.Select(x =>
			                                    {
			                                    	Permission permission;
			                                    	return x.TryGet(operation, out permission) ? permission : null;
			                                    }).Where(x => x != null).ToList();
			if (permissions.Count == 0)
			{
				return new AuditResult
				       {
				       	Granted = false,
				       	Permissions = permissions,
				       	Operation = operation,
				       	User = user
				       };
			}

			// get the permission with the highest priority
			var highestPriority = permissions.Max(x => x.Priority);

			// get the relevant permissions which are the permission with the highest priority
			var relevantPermissions = permissions.Where(x => highestPriority.Equals(x.Priority)).ToList();

			// check whether all the relevant permission grant access to the operation (AND).
			var hasPermission = relevantPermissions.All(x => x.Granted);

			// return the audit result
			return new AuditResult
			       {
			       	Granted = hasPermission,
			       	Permissions = relevantPermissions,
			       	Operation = operation,
			       	User = user
			       };
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="ISecurityPersistenceService"/>.
		/// </summary>
		private ISecurityPersistenceService PersistenceService
		{
			get { return persistenceService; }
		}
		#endregion
		#region Private Fields
		private readonly ISecurityPersistenceService persistenceService;
		#endregion
	}
}