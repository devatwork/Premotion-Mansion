using System;
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
		/// <param name="record"> </param>
		/// <param name="properties">The properties from which the <paramref name="record"/> was constructed.</param>
		protected override void DoAfterCreate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// retrieve the content index root node
			var contentIndexRootNode = context.Repository.RetrieveContentIndexRootNode(context);

			// get the node
			var node = record as Node;
			if (node == null)
				throw new InvalidOperationException("Record is not a node");

			// create a new node on this node
			context.Repository.CreateNode(context, node, new PropertyBag
			                                         {
			                                         	{"type", "TemplatePage"},
			                                         	{"name", "Default detail page"},
			                                         	{"contentSourceGuid", contentIndexRootNode.PermanentId},
			                                         	{"layout", node.Get<string>(context, "layout")},
			                                         });
		}
	}
}