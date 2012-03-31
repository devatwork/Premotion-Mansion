using System;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags
{
	/// <summary>
	/// Declares a procedure.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "declareProcedure")]
	public class DeclareProcedureTag : AlternativeScriptTag
	{
		#region Initialize Methods
		/// <summary>
		/// Initializes this tag in the correct context.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="tagScript">The script to which the tag belongs.</param>
		public override void InitializeContext(IMansionContext context, TagScript tagScript)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (tagScript == null)
				throw new ArgumentNullException("tagScript");

			// allow base to proces as well
			base.InitializeContext(context, tagScript);

			// get the name of the procedure
			var procedureName = GetRequiredAttribute<string>(context, "procedureName");

			// register the procedure
			tagScript.RegisterProcedure(context, procedureName, this);
		}
		#endregion
	}
}