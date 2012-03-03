using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Retrieves the template page for the specified source node.
	/// </summary>
	[Named(Constants.TagNamespaceUri, "retrieveTemplatePageNode")]
	public class RetrieveTemplatePageNodeTag : RetrieveNodeBaseTag
	{
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override Node Retrieve(MansionContext context, IPropertyBag arguments, IRepository repository)
		{
			// get the node
			var contentNode = GetRequiredAttribute<Node>(context, "source");
			var siteNode = GetRequiredAttribute<Node>(context, "siteNode");

			// get the portal service
			var portalService = context.Nucleus.Get<IPortalService>(context);

			// resolve the template page node
			Node templatePageNode;
			portalService.TryResolveTemplatePage(context, siteNode, contentNode, out templatePageNode);
			return templatePageNode;
		}
	}
}