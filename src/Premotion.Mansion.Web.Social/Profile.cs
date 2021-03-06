using System;
using System.Linq;

namespace Premotion.Mansion.Web.Social
{
	/// <summary>
	/// A normalized model representing a social service provider user profile.
	/// </summary>
	/// <remarks>
	/// The structure of a "UserProfile" varies across social service providers (see the difference between Facebook and Twitter, for example).
	/// That said, there are generally a common set of profile fields that apply across social service providers.
	/// This model provides access to those common fields in an uniform way.
	/// </remarks>
	public class Profile
	{
		#region Constants
		private readonly string[] commonSingleWordPrefixesList = new[] {"van", "de", "der", "den"};
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs an <see cref="Profile"/>.
		/// </summary>
		/// <param name="id">The user's social service provider id.</param>
		/// <param name="name">optional: The user's registered full name</param>
		/// <param name="firstName">optional: The user's registered first name.</param>
		/// <param name="lastNamePrefix">optional: the user's registered last name prefix.</param>
		/// <param name="lastName">optional: The user's registered last name.</param>
		/// <param name="gender">optional: The user's gender. </param>
		/// <param name="email">optional: The user's registered e-mail address.</param>
		/// <param name="username">optional: The user's registered username.</param>
		public Profile(SocialId id, string name = null, string firstName = null, string lastNamePrefix = null, string lastName = null, string gender = null, string email = null, string username = null)
		{
			// validate arguments
			if (id == null)
				throw new ArgumentNullException("id");

			// set values
			this.id = id.ToString();
			Name = name ?? string.Empty;
			FirstName = firstName ?? string.Empty;
			LastNamePrefix = lastNamePrefix ?? string.Empty;
			LastName = lastName ?? string.Empty;
			Gender = Gender ?? "unknown";
			this.email = email ?? string.Empty;
			this.username = username ?? string.Empty;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Normalizes the data in the profile.
		/// </summary>
		/// <returns>Returns the normalized profile for methog chaining.</returns>
		public Profile Normalize()
		{
			// assemble full name when empty and first & lastname are specified
			if (string.IsNullOrEmpty(Name) && (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)))
			{
				if (string.IsNullOrEmpty(LastNamePrefix))
					Name = FirstName + " " + LastName;
				else
					Name = FirstName + " " + LastNamePrefix + " " + LastName;
			}

			// if firstname & lastname are empty and name is not try to normalize them from name
			if (!string.IsNullOrEmpty(Name) && (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName)))
			{
				// split the name
				var nameParts = Name.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(part => part.Trim()).ToArray();

				// if the name is split in two assume it is a firstname/lastname pair
				if (nameParts.Length == 2)
				{
					FirstName = nameParts[0];
					LastName = nameParts[1];
				}

				// if the name is split in more than two parts, check if the middle parts are all known lastname prefix
				if (nameParts.Length > 2 && nameParts.Skip(1).Take(nameParts.Length - 2).All(candidate => commonSingleWordPrefixesList.Contains(candidate)))
				{
					FirstName = nameParts[0];
					LastNamePrefix = nameParts[nameParts.Length - 2];
					LastName = nameParts[nameParts.Length - 1];
				}
			}

			return this;
		}
		#endregion
		#region Properties
		/// <summary>
		/// The user's social service provider id.
		/// </summary>
		public String Id
		{
			get { return id; }
		}
		/// <summary>
		/// The user's registered full name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// The user's registered first name.
		/// </summary>
		public string FirstName { get; private set; }
		/// <summary>
		/// The user's registered last name prefix.
		/// </summary>
		public string LastNamePrefix { get; private set; }
		/// <summary>
		/// The user's registered last name.
		/// </summary>
		public string LastName { get; private set; }
		/// <summary>
		/// The user's gender. Either unknown, male or female.
		/// </summary>
		public string Gender { get; private set; }
		/// <summary>
		/// The user's registered e-mail address.
		/// </summary>
		public string Email
		{
			get { return email; }
		}
		/// <summary>
		/// The user's registered username.
		/// </summary>
		public string Username
		{
			get { return username; }
		}
		#endregion
		#region Private Fields
		private readonly string email;
		private readonly string id;
		private readonly string username;
		#endregion
	}
}