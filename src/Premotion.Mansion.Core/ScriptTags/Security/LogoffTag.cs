using System;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptTags.Security
{
	/// <summary>
	/// Log the user off for the current request context.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "logoff")]
	public class LogoffTag : ScriptTag
	{
		#region Constructs
		/// <summary>
		/// 
		/// </summary>
		/// <param name="securityService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public LogoffTag(ISecurityService securityService)
		{
			// validate arguments
			if (securityService == null)
				throw new ArgumentNullException("securityService");

			//set values
			this.securityService = securityService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// log off
			securityService.Logoff(context);
		}
		#endregion
		#region Private Fields
		private readonly ISecurityService securityService;
		#endregion
	}
}