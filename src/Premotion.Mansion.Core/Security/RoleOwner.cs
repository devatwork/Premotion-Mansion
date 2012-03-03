using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represents a owner of <see cref="Roles"/>.
	/// </summary>
	[Serializable]
	public abstract class RoleOwner
	{
		#region Constructors
		/// <summary>
		/// Constructs a role owner.
		/// </summary>
		/// <param name="id">The ID of the role owner.</param>
		protected RoleOwner(string id)
		{
			// validate arguments
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");

			// set value
			this.id = id;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Adds a <paramref name="role"/> to this <see cref="RoleOwner"/>.
		/// </summary>
		/// <param name="role">The <see cref="Role"/>.</param>
		public void Add(Role role)
		{
			// validate arguments
			if (role == null)
				throw new ArgumentNullException("role");
			roles.Add(role.Id, role);
		}
		/// <summary>
		/// Removes a <paramref name="role"/> from this <see cref="RoleOwner"/>.
		/// </summary>
		/// <param name="role">The <see cref="Role"/>.</param>
		public void Remove(Role role)
		{
			// validate arguments
			if (role == null)
				throw new ArgumentNullException("role");
			roles.Remove(role.Id);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this role owner. This is also known as the foreignId.
		/// </summary>
		public string Id
		{
			get { return id; }
		}
		/// <summary>
		/// Gets the <see cref="Role"/>s owned by this object.
		/// </summary>
		public IEnumerable<Role> Roles
		{
			get { return roles.Values; }
		}
		#endregion
		#region Private Fields
		private readonly string id;
		private readonly Dictionary<string, Role> roles = new Dictionary<string, Role>();
		#endregion
	}
}