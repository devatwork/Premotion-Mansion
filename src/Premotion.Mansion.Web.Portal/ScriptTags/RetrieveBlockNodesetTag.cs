using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Retrieves the block nodes for a node.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "retrieveBlockNodeset")]
	public class RetrieveBlockNodesetTag : RetrieveDatasetBaseTag
	{
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository)
		{
			// get the node
			var sourceNode = GetRequiredAttribute<Node>(context, "source");

			// retrieve the block nodes
			return repository.Retrieve(context, repository.ParseQuery(context, new PropertyBag
			                                                                   {
			                                                                   	{"baseType", "Block"},
			                                                                   	{"parentSource", sourceNode},
			                                                                   	{"status", NodeStatus.Published}
			                                                                   }));
		}
	}
}