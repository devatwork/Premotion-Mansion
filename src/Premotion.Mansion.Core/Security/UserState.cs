using System;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represent an user.
	/// </summary>
	[Serializable]
	public class UserState
	{
		#region Constructors
		/// <summary>
		/// Constructs an anonymous user.
		/// </summary>
		private UserState()
		{
			id = "Anonymous";
		}
		/// <summary>
		/// Constructs an authenicated user.
		/// </summary>
		/// <param name="id">The ID of the authenticated user.</param>
		/// <param name="authenticationProvider">The <see cref="AuthenticationProvider"/> which authenticated this user.</param>
		public UserState(string id, AuthenticationProvider authenticationProvider)
		{
			// validate arguments
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");
			if (authenticationProvider == null)
				throw new ArgumentNullException("authenticationProvider");

			// set values
			this.id = id;
			isAuthenticated = true;
			authenticationProviderName = authenticationProvider.Name;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this user.
		/// </summary>
		public string Id
		{
			get { return id; }
		}
		/// <summary>
		/// Gets the properties of this user.
		/// </summary>
		public IPropertyBag Properties
		{
			get { return properties; }
		}
		/// <summary>
		/// Gets a flag indicating whether this user is authenticated or not.
		/// </summary>
		public bool IsAuthenticated
		{
			get { return isAuthenticated; }
		}
		/// <summary>
		/// Gets the name of the authentication provider which authenticated this user.
		/// </summary>
		public string AuthenticationProviderName
		{
			get
			{
				if (string.IsNullOrEmpty(authenticationProviderName))
					throw new InvalidOperationException("This user does not have an authetication provider.");
				return authenticationProviderName;
			}
		}
		#endregion
		#region Singleton Implementation
		private static readonly UserState anonymousUser = new UserState();
		/// <summary>
		/// Get the anonymous user account.
		/// </summary>
		public static UserState AnonymousUser
		{
			get { return anonymousUser; }
		}
		#endregion
		#region Private Fields
		private readonly string authenticationProviderName;
		private readonly string id;
		private readonly bool isAuthenticated;
		private readonly IPropertyBag properties = new PropertyBag();
		#endregion
	}
}