using System;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptTags.Security
{
	/// <summary>
	/// Tries to authenticate an user for the current request context. 
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "authenticateUser")]
	public class AuthenticateUserTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="securityService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public AuthenticateUserTag(ISecurityService securityService)
		{
			// validate arguments
			if (securityService == null)
				throw new ArgumentNullException("securityService");

			// set values
			this.securityService = securityService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the attributes
			var attributes = GetAttributes(context);
			string authenticationProvider;
			if (!attributes.TryGetAndRemove(context, "authenticationProvider", out authenticationProvider) || string.IsNullOrEmpty(authenticationProvider))
				throw new InvalidOperationException("The attribute authenticationProvider can not be null or empty");

			// try to authenticate the user
			if (securityService.Authenticate(context, authenticationProvider, attributes))
				ExecuteChildTags(context);
			else
			{
				FailedTag failedTag;
				if (TryGetAlternativeChildTag(out failedTag))
					failedTag.Execute(context);
			}
		}
		#endregion
		#region Private Fields
		private readonly ISecurityService securityService;
		#endregion
	}
}