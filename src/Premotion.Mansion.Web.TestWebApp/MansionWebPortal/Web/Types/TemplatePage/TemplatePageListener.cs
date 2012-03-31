using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;
using Premotion.Mansion.Web.Portal.Descriptors;

namespace Premotion.Mansion.Web.Portal.Web.Types.TemplatePage
{
	/// <summary>
	/// This listener manages the position of blocks when a page is changing layouts.
	/// </summary>
	public class TemplatePageListener : NodeListener
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
			// get the layout schema
			string layoutName;
			if (!node.TryGet(context, "layout", out layoutName))
				throw new InvalidOperationException(string.Format("Template page {0} ({1}) does not have a layout", node.Pointer.PathString, node.Pointer.PointerString));
			var layoutSchema = ColumnSchema.GetSchema(context, layoutName);

			// add the content detail block to the primary column
			context.Repository.Create(context, node, new PropertyBag
			                                         {
			                                         	{"name", "Content Detail Block"},
			                                         	{"type", "ContentDetailBlock"},
			                                         	{"approved", true},
			                                         	{"column", layoutSchema.DefaultColumn}
			                                         });

			// invoke base
			base.DoAfterCreate(context, parent, node, newProperties);
		}
	}
}