using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;

namespace Premotion.Mansion.Web.Portal.Web.Types.Site
{
	/// <summary>
	/// This listener creates a default template page.
	/// </summary>
	public class SiteCreatedListener : NodeListener
	{
		/// <summary>
		/// This method is called just after a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child was be added.</param>
		/// <param name="node">The created node.</param>
		/// <param name="newProperties">The properties from which the <paramref name="node"/> was constructed.</param>
		protected override void DoAfterCreate(IMansionContext context, Node parent, Node node, IPropertyBag newProperties)
		{
			// retrieve the content index root node
			var contentIndexRootNode = context.Repository.RetrieveContentIndexRootNode(context);

			// create a new node on this node
			context.Repository.Create(context, node, new PropertyBag
			                                         {
			                                         	{"type", "TemplatePage"},
			                                         	{"name", "Default detail page"},
			                                         	{"contentSourceGuid", contentIndexRootNode.PermanentId},
			                                         	{"layout", node.Get<string>(context, "layout")},
			                                         });
		}
	}
}