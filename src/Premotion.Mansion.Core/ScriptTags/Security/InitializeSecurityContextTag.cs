using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptTags.Security
{
	/// <summary>
	/// Initializes the security for the specified <see cref="MansionContext"/>.
	/// </summary>
	[Named(Constants.NamespaceUri, "initializeSecurityContext")]
	public class InitializeSecurityContextTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// initializes the security context
			context.Nucleus.Get<ISecurityService>(context).InitializeSecurityContext(context);
		}
	}
}