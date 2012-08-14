using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Retrieves the theme node for the specified source node.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "retrieveThemeNode")]
	public class RetrieveThemeNodeTag : RetrieveRecordBaseTag
	{
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override IPropertyBag Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository)
		{
			// get the node
			var contentNode = GetRequiredAttribute<Node>(context, "source");

			// check if this node defines a theme
			string theme;
			if (contentNode.TryGet(context, "theme", out theme) && !string.IsNullOrEmpty(theme))
				return contentNode;

			// retrieve the parents of the current node
			var parentNodeset = repository.RetrieveNodeset(context, repository.ParseQuery(context, new PropertyBag
			                                                                                {
			                                                                                	{"childSource", contentNode},
			                                                                                	{"baseType", "Page"},
			                                                                                	{"depth", "any"},
			                                                                                	{"sort", "depth DESC"}
			                                                                                }));

			return parentNodeset.Nodes.FirstOrDefault(parent => parent.TryGet(context, "theme", out theme) && !string.IsNullOrEmpty(theme));
		}
	}
}