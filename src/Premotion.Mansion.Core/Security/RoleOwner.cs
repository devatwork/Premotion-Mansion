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
		/// <param name="id">The permanent ID of the role owner.</param>
		protected RoleOwner(Guid id)
		{
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
		/// Gets the permanent ID of this role owner.
		/// </summary>
		public Guid Id
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
		private readonly Guid id;
		private readonly Dictionary<Guid, Role> roles = new Dictionary<Guid, Role>();
		#endregion
	}
}