using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;
using Premotion.Mansion.Web.Portal.Descriptors;

namespace Premotion.Mansion.Web.Portal.Web.Types.PresentationPage
{
	/// <summary>
	/// This listener manages the position of blocks when a page is changing layouts.
	/// </summary>
	public class PresentationPageListener : NodeListener
	{
		/// <summary>
		/// This method is called just after a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The properties from which the <paramref name="record"/> was constructed.</param>
		protected override void DoAfterCreate(IMansionContext context, Record record, IPropertyBag properties)
		{
			var node = record as Node;
			if (node == null)
				throw new InvalidOperationException("Record is not a node");

			// get the layout schema
			string layoutName;
			if (!node.TryGet(context, "layout", out layoutName))
				throw new InvalidOperationException(string.Format("Presentation page {0} ({1}) does not have a layout", node.Pointer.PathString, node.Pointer.PointerString));
			var layoutSchema = ColumnSchema.GetSchema(context, layoutName);

			// add the content detail block to the primary column
			context.Repository.CreateNode(context, node, new PropertyBag
			                                             {
			                                             	{"name", "Page Presentation Block"},
			                                             	{"type", "PresentationPageBlock"},
			                                             	{"approved", true},
			                                             	{"column", layoutSchema.DefaultColumn},
			                                             	{"borderStyle", "border"},
			                                             	{"blockStyle", "normal"}
			                                             });
		}
	}
}