using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptTags.Security
{
	/// <summary>
	/// Tries to authenticate an user for the current request context. 
	/// </summary>
	[Named(Constants.NamespaceUri, "authenticateUser")]
	public class AuthenticateUserTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the attributes
			var attributes = GetAttributes(context);
			string authenticationProvider;
			if (!attributes.TryGetAndRemove(context, "authenticationProvider", out authenticationProvider) || string.IsNullOrEmpty(authenticationProvider))
				throw new InvalidOperationException("The attribute authenticationProvider can not be null or empty");

			// get the security service
			var securityService = context.Nucleus.Get<ISecurityService>(context);

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
	}
}