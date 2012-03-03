using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptTags.Security
{
	/// <summary>
	/// Log the user off for the current request context.
	/// </summary>
	[Named(Constants.NamespaceUri, "logoff")]
	public class LogoffTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the security service
			var securityService = context.Nucleus.Get<ISecurityService>(context);

			// log off
			securityService.Logoff(context);
		}
	}
}