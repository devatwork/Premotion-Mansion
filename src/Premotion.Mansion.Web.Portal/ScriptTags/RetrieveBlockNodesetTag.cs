using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Repository;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Retrieves the block nodes for a node.
	/// </summary>
	[Named(Constants.TagNamespaceUri, "retrieveBlockNodeset")]
	public class RetrieveBlockNodesetTag : RetrieveNodesetBaseTag
	{
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override Nodeset Retrieve(MansionContext context, IPropertyBag arguments, IRepository repository)
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