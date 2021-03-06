﻿using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
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
		/// <returns>Returns the <see cref="AuthenticationResult"/>.</returns>
		protected override AuthenticationResult DoAuthenticate(IMansionContext context, IPropertyBag parameters)
		{
			return parameters.Contains("userId") ? AuthenticateWithId(context, parameters) : AuthenticateWithUsernamePassword(context, parameters);
		}
		/// <summary>
		/// Authenticates using ID.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		private AuthenticationResult AuthenticateWithId(IMansionContext context, IPropertyBag parameters)
		{
			// get the credentials
			var userId = parameters.Get<int>(context, "userId");

			// perform a query
			var userNode = context.Repository.RetrieveSingleNode(context, new PropertyBag {
				{"baseType", "User"},
				{"id", userId},
				{"status", "any"},
				{"bypassAuthorization", true},
				{"cache", false},
				{StorageOnlyQueryComponent.PropertyKey, true}
			});
			if (userNode == null)
			{
				return AuthenticationResult.Failed(new PropertyBag {
					{AuthenticationResult.ReasonPropertyName, AuthenticationResult.InvalidCredentialsReason}
				});
			}

			// create and return the user state
			return AuthenticationResult.Success(CreateUserState(context, userNode), new PropertyBag());
		}
		/// <summary>
		/// Authenticates using username/password.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		private AuthenticationResult AuthenticateWithUsernamePassword(IMansionContext context, IPropertyBag parameters)
		{
			// get the credentials
			var username = parameters.Get(context, "username", string.Empty);
			if (string.IsNullOrEmpty(username))
			{
				return AuthenticationResult.Failed(new PropertyBag {
					{AuthenticationResult.ReasonPropertyName, AuthenticationResult.NoUsernameSpecifiedReason}
				});
			}
			var password = parameters.Get(context, "password", string.Empty);
			if (string.IsNullOrEmpty(password))
			{
				return AuthenticationResult.Failed(new PropertyBag {
					{AuthenticationResult.ReasonPropertyName, AuthenticationResult.NoPasswordSpecifiedReason}
				});
			}

			// perform a query
			var userNode = context.Repository.RetrieveSingleNode(context, new PropertyBag {
				{"baseType", "User"},
				{"login", username},
				{"password", password},
				{"status", "any"},
				{"bypassAuthorization", true},
				{"cache", false},
				{StorageOnlyQueryComponent.PropertyKey, true}
			});
			if (userNode == null)
			{
				return AuthenticationResult.Failed(new PropertyBag {
					{AuthenticationResult.ReasonPropertyName, AuthenticationResult.InvalidCredentialsReason}
				});
			}

			// check against unpublished users
			if (userNode.Status != NodeStatus.Published)
			{
				return AuthenticationResult.Failed(new PropertyBag {
					{AuthenticationResult.ReasonPropertyName, AuthenticationResult.AccounDeactivatedReason}
				});
			}

			// create and return the user state
			return AuthenticationResult.Success(CreateUserState(context, userNode), new PropertyBag());
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
			return new PropertyBag {
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
			Guid id;
			if (!revivalProperties.TryGet(context, "id", out id))
				return null;

			// retrieve the user by guid
			var userNode = context.Repository.RetrieveSingleNode(context, new PropertyBag {
				{"baseType", "User"},
				{"guid", id},
				{"status", "any"},
				{"bypassAuthorization", true},
				{"cache", false},
				{StorageOnlyQueryComponent.PropertyKey, true}
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
			var userState = new UserState(userNode.Get<Guid>(context, "guid"), this);

			// merge the user properties
			userState.Properties.Merge(userNode);

			return userState;
		}
		#endregion
	}
}