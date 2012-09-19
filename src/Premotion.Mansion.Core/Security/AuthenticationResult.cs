using System;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Contains the result of an authentication.
	/// </summary>
	public class AuthenticationResult
	{
		#region Constants
		/// <summary>
		/// The name of the property in which the reason is stored.
		/// </summary>
		public const string ReasonPropertyName = "reason";
		/// <summary>
		/// The username was not specified.
		/// </summary>
		public const string NoUsernameSpecifiedReason = "no-username-specified";
		/// <summary>
		/// The password was not specified.
		/// </summary>
		public const string NoPasswordSpecifiedReason = "no-password-specified";
		/// <summary>
		/// Invalid credentials were specified.
		/// </summary>
		public const string InvalidCredentialsReason = "invalid-credentials";
		#endregion
		#region Constructors
		/// <summary>
		/// Private
		/// </summary>
		private AuthenticationResult(IPropertyBag properties, UserState userState = null)
		{
			// set values
			this.userState = userState;
			this.properties = properties;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates a succesful <see cref="AuthenticationResult"/>.
		/// </summary>
		/// <param name="authenticatedUserState">The <see cref="UserState"/> of the authenticated user.</param>
		/// <param name="successProperties">The properties indicating the reason of the succesful authentication.</param>
		/// <returns>Returns the created <see cref="AuthenticationResult"/></returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static AuthenticationResult Success(UserState authenticatedUserState, IPropertyBag successProperties)
		{
			// validate arguments
			if (authenticatedUserState == null)
				throw new ArgumentNullException("authenticatedUserState");
			if (successProperties == null)
				throw new ArgumentNullException("successProperties");

			// create the result
			return new AuthenticationResult(successProperties, authenticatedUserState);
		}
		/// <summary>
		/// Creates a failed <see cref="AuthenticationResult"/>.
		/// </summary>
		/// <param name="failureProperties">The properties indicating the reason of the failure.</param>
		/// <returns>Returns the created <see cref="AuthenticationResult"/></returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static AuthenticationResult Failed(IPropertyBag failureProperties)
		{
			// validate arguments
			if (failureProperties == null)
				throw new ArgumentNullException("failureProperties");

			// create the result
			return new AuthenticationResult(failureProperties);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether the authentication was succesful or not.
		/// </summary>
		public bool WasSuccesful
		{
			get { return userState != null; }
		}
		/// <summary>
		/// Gets the <see cref="UserState"/> of this authentication result.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="WasSuccesful"/> is false.</exception>
		public UserState UserState
		{
			get
			{
				// guard against null
				if (userState == null)
					throw new InvalidOperationException("The authentication was not successful so the state is null.");
				return userState;
			}
		}
		/// <summary>
		/// Gets the properties of this <see cref="AuthenticationResult"/>.
		/// </summary>
		public IPropertyBag Properties
		{
			get { return properties; }
		}
		#endregion
		#region Private Fields
		private readonly IPropertyBag properties;
		private readonly UserState userState;
		#endregion
	}
}