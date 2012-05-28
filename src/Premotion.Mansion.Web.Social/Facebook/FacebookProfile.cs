namespace Premotion.Mansion.Web.Social.Facebook
{
	/// <summary>
	/// Represents a Facebook user profile.
	/// </summary>
	public class FacebookProfile : ModelBase
	{
		#region Map Methods
		/// <summary>
		/// Maps the <see cref="FacebookProfile"/> to <see cref="Profile"/>.
		/// </summary>
		/// <returns>Returns the mapped <see cref="Profile"/>.</returns>
		public Profile Map()
		{
			return new Profile(new SocialId(Id, Constants.ProviderName), Name, FirstName, MiddleName, LastName, Gender, Email, Username).Normalize();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of the profile.
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// Gets the gender of the profile. Either male or female.
		/// </summary>
		public string Gender { get; set; }
		/// <summary>
		/// Gets the name of the profile.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gets the first name of the profile.
		/// </summary>
		public string FirstName { get; set; }
		/// <summary>
		/// Gets the middle name of the profile.
		/// </summary>
		public string MiddleName { get; set; }
		/// <summary>
		/// Gets the first name of the profile.
		/// </summary>
		public string LastName { get; set; }
		/// <summary>
		/// Gets the e-mail of the profile.
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// Gets the username of the profile.
		/// </summary>
		public string Username { get; set; }
		#endregion
	}
}