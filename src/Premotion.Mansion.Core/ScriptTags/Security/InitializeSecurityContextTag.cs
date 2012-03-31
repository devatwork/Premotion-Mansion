using System;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptTags.Security
{
	/// <summary>
	/// Initializes the security for the specified <see cref="IMansionContext"/>.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "initializeSecurityContext")]
	public class InitializeSecurityContextTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="securityService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public InitializeSecurityContextTag(ISecurityService securityService)
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
			// initializes the security context
			securityService.InitializeSecurityContext(context);
		}
		#endregion
		#region Private Fields
		private readonly ISecurityService securityService;
		#endregion
	}
}