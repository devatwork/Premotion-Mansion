using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Sets the status code for the current response.
	/// </summary>
	[Named(Constants.NamespaceUri, "setStatusCode")]
	public class SetStatusCodeTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			// get the web context
			var webContext = context.Cast<MansionWebContext>();

			// get the status code
			webContext.HttpContext.Response.StatusCode = GetRequiredAttribute<int>(context, "code");
			webContext.HttpContext.Response.StatusDescription = GetAttribute<string>(context, "description");
		}
		#endregion
	}
}