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
		/// <param name="id">The ID of the user.</param>
		public User(string id) : base(id)
		{
		}
		#endregion
	}
}