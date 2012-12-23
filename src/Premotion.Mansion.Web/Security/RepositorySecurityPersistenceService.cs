using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Web.Security
{
	/// <summary>
	/// Provides an implemenation for <see cref="ISecurityPersistenceService"/> using a repository backed storage.
	/// </summary>
	public class RepositorySecurityPersistenceService : ISecurityPersistenceService
	{
		#region Constants
		private const string GrantedPostfix = "_granted";
		#endregion
		#region Implementation of ISecurityPersistenceService
		/// <summary>
		/// Creates the new <paramref name="permission"/> on the <paramref name="role"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="role">The <see cref="Role"/>.</param>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		public void CreatePermission(IMansionContext context, Role role, Permission permission)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (role == null)
				throw new ArgumentNullException("role");
			if (permission == null)
				throw new ArgumentNullException("permission");

			// get the repository
			var repository = context.Repository;

			// retrieve the role
			var roleNode = RetrieveRoleNode(context, role, repository);

			// store the permission
			var permissionPrefix = permission.Operation.Resource.Id + "_" + permission.Operation.Id + "_";
			repository.UpdateNode(context, roleNode, new PropertyBag
			                                         {
			                                         	{permissionPrefix + "granted", permission.Granted},
			                                         	{permissionPrefix + "priority", permission.Priority},
			                                         });
		}
		/// <summary>
		/// Updates an existing <paramref name="permission"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		public void UpdatePermission(IMansionContext context, Permission permission)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (permission == null)
				throw new ArgumentNullException("permission");

			throw new System.NotImplementedException();
		}
		/// <summary>
		/// Deletes the <paramref name="permission"/> from the <paramref name="role"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="role">The <see cref="Role"/>.</param>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		public void DeletePermission(IMansionContext context, Role role, Permission permission)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (role == null)
				throw new ArgumentNullException("role");

			throw new System.NotImplementedException();
		}
		/// <summary>
		/// Adds a <paramref name="role"/> to the <paramref name="owner"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="owner">The <see cref="RoleOwner"/>.</param>
		/// <param name="role">The <see cref="Role"/>.</param>
		public void AssignRole(IMansionContext context, RoleOwner owner, Role role)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (owner == null)
				throw new ArgumentNullException("owner");
			if (role == null)
				throw new ArgumentNullException("role");

			// get the repository
			var repository = context.Repository;

			// retrieve the required nodes
			var ownerNode = RetrieveRoleOwnerNode(context, owner, repository);
			var roleNode = RetrieveRoleNode(context, role, repository);

			// update the role owner
			repository.UpdateNode(context, ownerNode, new PropertyBag
			                                          {
			                                          	{"assignedRoleGuids", string.Join(",", new[] {ownerNode.Get(context, "assignedRoleGuids", string.Empty), roleNode.Get<string>(context, "guid")})}
			                                          });
		}
		/// <summary>
		/// Removes a <paramref name="role"/> from the <paramref name="owner"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="owner">The <see cref="RoleOwner"/>.</param>
		/// <param name="role">The <see cref="Role"/>.</param>
		public void RemoveRole(IMansionContext context, RoleOwner owner, Role role)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (owner == null)
				throw new ArgumentNullException("owner");
			if (role == null)
				throw new ArgumentNullException("role");

			// get the repository
			var repository = context.Repository;

			// retrieve the required nodes
			var ownerNode = RetrieveRoleOwnerNode(context, owner, repository);
			var roleNode = RetrieveRoleNode(context, role, repository);

			// build the userGuids array
			var assignedRoleList = (ownerNode.Get(context, "assignedRoleGuids", string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)).ToList();
			assignedRoleList.Remove(roleNode.Get<string>(context, "guid"));

			// update the user group
			repository.UpdateNode(context, ownerNode, new PropertyBag
			                                          {
			                                          	{"assignedRoleGuids", string.Join(",", assignedRoleList)}
			                                          });
		}
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

			// get the repository
			var repository = context.Repository;

			// retrieve the required nodes
			var userNode = RetrieveRoleOwnerNode(context, user, repository);
			var groupNode = RetrieveRoleOwnerNode(context, group, repository);

			// update the user group
			repository.UpdateNode(context, groupNode, new PropertyBag
			                                          {
			                                          	{"userGuids", string.Join(",", new[] {groupNode.Get(context, "userGuids", string.Empty), userNode.Get<string>(context, "guid")})}
			                                          });
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

			// get the repository
			var repository = context.Repository;

			// retrieve the required nodes
			var userNode = RetrieveRoleOwnerNode(context, user, repository);
			var groupNode = RetrieveRoleOwnerNode(context, group, repository);

			// build the userGuids array
			var userGuidsList = (groupNode.Get(context, "userGuids", string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)).ToList();
			userGuidsList.Remove(userNode.Get<string>(context, "guid"));

			// update the user group
			repository.UpdateNode(context, groupNode, new PropertyBag
			                                          {
			                                          	{"userGuids", string.Join(",", userGuidsList)}
			                                          });
		}
		/// <summary>
		/// Retrieves a <see cref="User"/> by it's ID.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/></param>
		/// <param name="id">The ID of the <see cref="User"/>.</param>
		/// <returns>Returns the loaded <see cref="User"/>.</returns>
		public User RetrieveUser(IMansionContext context, Guid id)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// create the user
			var user = new User(id);

			// get the repository
			var repository = context.Repository;

			// first, retrieve the user node
			var userNode = RetrieveRoleOwnerNode(context, user, repository);

			// second, retrieve the groups the user is in
			var groupNodes = RetrieveUserGroupNodes(context, userNode, repository);

			// assemble the role select criteria
			var assignedRoleGuidsUser = userNode.Get(context, "assignedRoleGuids", string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
			var assignedRoleGuidsGroups = groupNodes.Nodes.SelectMany(group => group.Get(context, "assignedRoleGuids", string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries));
			var assignedRoleGuids = assignedRoleGuidsUser.Union(assignedRoleGuidsGroups).Distinct();

			// retrieve the roles
			var roleNodes = RetrieveRoleNodes(context, assignedRoleGuids, repository);

			// turn the roles nodes into roles
			foreach (var roleNode in roleNodes.Nodes)
				user.Add(MapRole(context, roleNode));

			// return the user
			return user;
		}
		#endregion
		#region Retrieve Methods
		/// <summary>
		/// Retrieves the role owner node.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="owner"></param>
		/// <param name="repository"></param>
		/// <returns></returns>
		private static Node RetrieveRoleOwnerNode(IMansionContext context, RoleOwner owner, IRepository repository)
		{
			// if the user is not authenticated, retrieve the visiter from the database
			Node node;
			if (owner.Id == Guid.Empty)
			{
				node = repository.RetrieveSingleNode(context, new PropertyBag
				                                              {
				                                              	{"baseType", "RoleOwner"},
				                                              	{"key", "AnonymousUser"},
				                                              	{"bypassAuthorization", true}
				                                              });
				if (node == null)
					throw new InvalidOperationException("Could not find the anonymous user node");
			}
			else
			{
				node = repository.RetrieveSingleNode(context, new PropertyBag
				                                              {
				                                              	{"baseType", "RoleOwner"},
				                                              	{"guid", owner.Id},
				                                              	{"bypassAuthorization", true}
				                                              });
				if (node == null)
					throw new InvalidOperationException(string.Format("Could not find role owner with foreign ID {0} in repository, please sync tables", owner.Id));
			}

			return node;
		}
		/// <summary>
		/// Retrieves a set of roles nodes by <paramref name="roleGuids"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="roleGuids"></param>
		/// <param name="repository"></param>
		/// <returns></returns>
		private Nodeset RetrieveRoleNodes(IMansionContext context, IEnumerable<string> roleGuids, IRepository repository)
		{
			return repository.RetrieveNodeset(context, new PropertyBag
			                                           {
			                                           	{"baseType", "Role"},
			                                           	{"guid", string.Join(",", roleGuids)},
			                                           	{"bypassAuthorization", true}
			                                           });
		}
		/// <summary>
		/// Retrieves the role node.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="role"></param>
		/// <param name="repository"></param>
		/// <returns></returns>
		private static Node RetrieveRoleNode(IMansionContext context, Role role, IRepository repository)
		{
			var node = repository.RetrieveSingleNode(context, new PropertyBag
			                                                  {
			                                                  	{"baseType", "Role"},
			                                                  	{"guid", role.Id},
			                                                  	{"bypassAuthorization", true}
			                                                  });
			if (node == null)
				throw new InvalidOperationException(string.Format("Could not find role with ID {0} in repository, please sync tables", role.Id));
			return node;
		}
		/// <summary>
		/// Retrieves a <see cref="Nodeset"/> containing the groups to which <paramref name="userNode"/> belongs.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="userNode"></param>
		/// <param name="repository"></param>
		/// <returns></returns>
		private Nodeset RetrieveUserGroupNodes(IMansionContext context, Node userNode, IRepository repository)
		{
			return repository.RetrieveNodeset(context, new PropertyBag
			                                           {
			                                           	{"baseType", "UserGroup"},
			                                           	{"userGuids", userNode.Get<string>(context, "guid")},
			                                           	{"bypassAuthorization", true}
			                                           });
		}
		#endregion
		#region Map Methods
		/// <summary>
		/// Maps a <paramref name="roleNode"/> to <see cref="Role"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="roleNode"></param>
		/// <returns></returns>
		private Role MapRole(IMansionContext context, Node roleNode)
		{
			// create the role
			var role = new Role(roleNode.PermanentId);

			// find all the properties ending with _granted
			foreach (var property in roleNode.Where(x => x.Key.EndsWith(GrantedPostfix, StringComparison.OrdinalIgnoreCase)))
			{
				// get the resourceId and operationId
				var permissionParts = property.Key.Substring(0, property.Key.Length - GrantedPostfix.Length).Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries);
				if (permissionParts.Length != 2)
					throw new InvalidOperationException(string.Format("Invalid permission '{0}' found in role '{1}'", property.Key, roleNode.Pointer.PathString));
				var resourceId = permissionParts[0];
				var operationId = permissionParts[1];
				var permissionPrefix = resourceId + "_" + operationId + "_";

				// create the operation
				var operation = ProtectedOperation.Create(context, resourceId, operationId);

				// create the permission
				var permission = new Permission
				                 {
				                 	Granted = roleNode.Get(context, permissionPrefix + "granted", false),
				                 	Operation = operation,
				                 	Priority = roleNode.Get(context, permissionPrefix + "priority", 5)
				                 };

				// add the permission to the role
				role.Add(permission);
			}

			// return the role
			return role;
		}
		#endregion
	}
}