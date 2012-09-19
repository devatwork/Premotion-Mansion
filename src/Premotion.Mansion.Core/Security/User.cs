using System;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represents a user.
	/// </summary>
	public class User : RoleOwner
	{
		#region Constructors
		/// <summary>
		/// Constructs a user.
		/// </summary>
		/// <param name="id">The permanent ID of the user.</param>
		public User(Guid id) : base(id)
		{
		}
		#endregion
	}
}