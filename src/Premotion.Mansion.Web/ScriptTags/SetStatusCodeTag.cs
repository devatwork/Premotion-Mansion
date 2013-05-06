using System.Net;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Sets the status code for the current response.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "setStatusCode")]
	public class SetStatusCodeTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the current output pipe
			var outputPipe = context.GetWebOuputPipe();

			// get the status code
			outputPipe.Response.StatusCode = (HttpStatusCode) GetRequiredAttribute<int>(context, "code");
			string description;
			if (TryGetAttribute(context, "description", out description))
				outputPipe.Response.StatusDescription = description;
		}
		#endregion
	}
}