﻿using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Provides persistence
	/// </summary>
	public interface ISecurityPersistenceService : IService
	{
		#region Permission Persistence Methods
		/// <summary>
		/// Creates the new <paramref name="permission"/> on the <paramref name="role"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="role">The <see cref="Role"/>.</param>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		void CreatePermission(MansionContext context, Role role, Permission permission);
		/// <summary>
		/// Updates an existing <paramref name="permission"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		void UpdatePermission(MansionContext context, Permission permission);
		/// <summary>
		/// Deletes the <paramref name="permission"/> from the <paramref name="role"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="role">The <see cref="Role"/>.</param>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		void DeletePermission(MansionContext context, Role role, Permission permission);
		#endregion
		#region Role Persistence Methods
		/// <summary>
		/// Adds a <paramref name="role"/> to the <paramref name="owner"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="owner">The <see cref="RoleOwner"/>.</param>
		/// <param name="role">The <see cref="Role"/>.</param>
		void AssignRole(MansionContext context, RoleOwner owner, Role role);
		/// <summary>
		/// Removes a <paramref name="role"/> from the <paramref name="owner"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="owner">The <see cref="RoleOwner"/>.</param>
		/// <param name="role">The <see cref="Role"/>.</param>
		void RemoveRole(MansionContext context, RoleOwner owner, Role role);
		#endregion
		#region User Group Persistance Methods
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
		#endregion
		#region User Methods
		/// <summary>
		/// Retrieves a <see cref="User"/> by it's ID.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/></param>
		/// <param name="id">The ID of the <see cref="User"/>.</param>
		/// <returns>Returns the loaded <see cref="User"/>.</returns>
		User RetrieveUser(MansionContext context, string id);
		#endregion
	}
}