using System.Collections.Generic;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represents a set of <see cref="User"/>s  who  share the same group.
	/// </summary>
	public class UserGroup : RoleOwner
	{
		#region Constructors
		/// <summary>
		/// Constructs a UserGroup.
		/// </summary>
		/// <param name="id">The ID of the user group.</param>
		public UserGroup(string id) : base(id)
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="User"/>s in this group.
		/// </summary>
		public IEnumerable<User> Users { get; set; }
		#endregion
	}
}