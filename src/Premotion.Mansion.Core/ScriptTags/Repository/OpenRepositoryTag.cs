using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Opens a connection to a repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "openRepository")]
	public class OpenRepositoryTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// open the repository
			using (RepositoryUtil.Open(context))
				ExecuteChildTags(context);
		}
		#endregion
	}
}