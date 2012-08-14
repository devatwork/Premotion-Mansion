using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Web.Security
{
	/// <summary>
	/// Implements <see cref="AuthenticationProvider"/> using the repository to validate credentials.
	/// </summary>
	public class RepositoryAuthenticationProvider : AuthenticationProvider
	{
		#region Constructors
		/// <summary>
		/// Constructs an authentication provider.
		/// </summary>
		public RepositoryAuthenticationProvider() : base("RepositoryAuthenticationProvider")
		{
		}
		#endregion
		#region Authentication Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <param name="parameters">The parameters used for authentication.</param>
		/// <returns>Returns the user when authenticated otherwise null.</returns>
		protected override UserState DoAuthenticate(IMansionContext context, IPropertyBag parameters)
		{
			// get the credentials
			var username = parameters.Get(context, "username", string.Empty);
			if (string.IsNullOrEmpty(username))
				return null;
			var password = parameters.Get(context, "password", string.Empty);
			if (string.IsNullOrEmpty(password))
				return null;

			// perform a query
			var userNode = context.Repository.RetrieveSingleNode(context, new PropertyBag
			                                                          {
			                                                          	{"baseType", "User"},
			                                                          	{"login", username},
			                                                          	{"password", password},
			                                                          	{"status", "any"},
			                                                          	{"bypassAuthorization", true},
			                                                          	{"cache", false}
			                                                          });
			if (userNode == null)
				return null;

			// create and return the user state
			return CreateUserState(context, userNode);
		}
		/// <summary>
		/// Logs the current user off.
		/// </summary>
		/// <param name="context">The security context.</param>
		protected override void DoLogoff(IMansionContext context)
		{
			// do nothing special
		}
		#endregion
		#region Revival Methods
		/// <summary>
		/// Gets the data used to revive users.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <param name="userState">The user which will be revived.</param>
		/// <param name="parameters">The parameters used for authentication of <paramref name="userState"/>.</param>
		/// <returns>Returns the revival data or null.</returns>
		/// <exception cref="NotSupportedException">Thrown when <see cref="AuthenticationProvider.SupportsRevival"/> is false.</exception>
		public override IPropertyBag GetRevivalProperties(IMansionContext context, UserState userState, IPropertyBag parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (userState == null)
				throw new ArgumentNullException("userState");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			// return the revival properties
			return new PropertyBag
			       {
			       	{"id", userState.Id}
			       };
		}
		/// <summary>
		/// Tries to revive the user based on the revival properties.
		/// </summary>
		/// <param name="context">The security context.</param>
		/// <param name="revivalProperties">The revival properties.</param>
		/// <returns>Returns the revived <see cref="UserState"/> or null.</returns>
		public override UserState ReviveUser(IMansionContext context, IPropertyBag revivalProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (revivalProperties == null)
				throw new ArgumentNullException("revivalProperties");

			// get the id of the from the revival properties
			string id;
			if (!revivalProperties.TryGet(context, "id", out id))
				return null;

			// retrieve the user by guid
			var userNode = context.Repository.RetrieveSingleNode(context, new PropertyBag
			                                                          {
			                                                          	{"baseType", "User"},
			                                                          	{"guid", id},
			                                                          	{"status", "any"},
			                                                          	{"bypassAuthorization", true},
			                                                          	{"cache", false}
			                                                          });
			if (userNode == null)
				return null;

			// create and return the user state
			return CreateUserState(context, userNode);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether this authentication provider supports user revival.
		/// </summary>
		public override bool SupportsRevival
		{
			get { return true; }
		}
		#endregion
		#region Helper Methods
		private UserState CreateUserState(IMansionContext context, Node userNode)
		{
			// create the user state
			var userState = new UserState(userNode.Get<string>(context, "guid"), this);

			// merge the user properties
			userState.Properties.Merge(userNode);

			return userState;
		}
		#endregion
	}
}