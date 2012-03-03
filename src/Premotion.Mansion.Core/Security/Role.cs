using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represents a role which grants <see cref="Permission"/>s.
	/// </summary>
	public class Role
	{
		#region Constructors
		/// <summary>
		/// Constructs a role.
		/// </summary>
		/// <param name="id">The ID of the role.</param>
		public Role(string id)
		{
			// validate arguments
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");

			// set values
			this.id = id;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Tries to get the <paramref name="permission"/> for the <paramref name="operation"/>.
		/// </summary>
		/// <param name="operation">The <see cref="ProtectedOperation"/>.</param>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		/// <returns>Returns true when a <paramref name="permission"/> was found for the <paramref name="operation"/>, otherwise false.</returns>
		public bool TryGet(ProtectedOperation operation, out Permission permission)
		{
			// validate arguments
			if (operation == null)
				throw new ArgumentNullException("operation");
			return permissions.TryGetValue(operation, out permission);
		}
		/// <summary>
		/// Adds the <paramref name="permission"/> to this <see cref="Role"/>.
		/// </summary>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		public void Add(Permission permission)
		{
			// validate arguments
			if (permission == null)
				throw new ArgumentNullException("permission");
			permissions.Add(permission.Operation, permission);
		}
		/// <summary>
		/// Removes the <paramref name="permission"/> from this <see cref="Role"/>.
		/// </summary>
		/// <param name="permission">The <see cref="Permission"/>.</param>
		public void Remove(Permission permission)
		{
			// validate arguments
			if (permission == null)
				throw new ArgumentNullException("permission");
			permissions.Remove(permission.Operation);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this <see cref="Role"/>.
		/// </summary>
		public string Id
		{
			get { return id; }
		}
		#endregion
		#region Private Fields
		private readonly string id;
		private readonly Dictionary<ProtectedOperation, Permission> permissions = new Dictionary<ProtectedOperation, Permission>();
		#endregion
	}
}