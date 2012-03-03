using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Opens a connection to a repository.
	/// </summary>
	[Named(Constants.NamespaceUri, "openRepository")]
	public class OpenRepositoryTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(MansionContext context)
		{
			// get the attributes
			var repositoryNamespace = GetRequiredAttribute<string>(context, "repositoryNamespace");
			var connectionString = GetRequiredAttribute<string>(context, "connectionString");

			// open the repository
			using (RepositoryUtil.Open(context, repositoryNamespace, connectionString))

				ExecuteChildTags(context);
		}
		#endregion
	}
}