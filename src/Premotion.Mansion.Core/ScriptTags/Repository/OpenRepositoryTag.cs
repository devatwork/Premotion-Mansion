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
			// get the attributes
			var repositoryNamespace = GetRequiredAttribute<string>(context, "repositoryNamespace");
			var applicationSettings = context.Stack.Peek<IPropertyBag>("Application");

			// open the repository
			using (RepositoryUtil.Open(context, repositoryNamespace, applicationSettings))
				ExecuteChildTags(context);
		}
		#endregion
	}
}