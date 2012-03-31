using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Fetches a node from the stack and puts it into a new dataspace.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "fetchNode")]
	public class FetchNodeTag : RetrieveNodeBaseTag
	{
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override Node Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository)
		{
			return GetAttribute<Node>(context, "source");
		}
	}
}