using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;
using Premotion.Mansion.Web.Portal.Descriptors;

namespace Premotion.Mansion.Web.Portal.Web.Types.Page
{
	/// <summary>
	/// This listener manages the position of blocks when a page is changing layouts.
	/// </summary>
	public class PageLayoutChangeListener : NodeListener
	{
		/// <summary>
		/// This method is called just before a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child will be added.</param>
		/// <param name="newProperties">The new properties of the node.</param>
		protected override void DoBeforeCreate(MansionContext context, Node parent, IPropertyBag newProperties)
		{
			// check if the layout is set
			string layout;
			if (newProperties.TryGet(context, "layout", out layout))
				return;
			if (!parent.TryGet(context, "layout", out layout))
				layout = "OneColumnLayout";

			// set the layout
			newProperties.TrySet("layout", layout);
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which will be modified.</param>
		/// <param name="modifiedProperties">The updated properties of the node.</param>
		protected override void DoBeforeUpdate(MansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// check if the layout was not modified
			string newLayoutName;
			if (!modifiedProperties.TryGet(context, "layout", out newLayoutName))
				return;

			// check if there was no old layout
			string oldLayoutName;
			if (!node.TryGet(context, "layout", out oldLayoutName))
				return;

			// get the schemas
			var newColumnSchema = ColumnSchema.GetSchema(context, newLayoutName);
			var oldColumnSchema = ColumnSchema.GetSchema(context, oldLayoutName);

			// retrieve the blocks of this page
			var repository = context.Repository;
			var blockNodeset = repository.Retrieve(context, repository.ParseQuery(context, new PropertyBag
			                                                                               {
			                                                                               	{"baseType", "Block"},
			                                                                               	{"parentSource", node}
			                                                                               }));

			// loop through all the nodes
			foreach (var blockNode in blockNodeset.Nodes)
			{
				// check if this block was not in a column of the old schema so it wont have to move to the new schema
				var columnName = blockNode.Get<string>(context, "column");
				if (!oldColumnSchema.ContainsColumn(columnName))
					continue;

				// check if the column is in the new schema as well so it wont have to move
				if (newColumnSchema.ContainsColumn(columnName))
					continue;

				// move the block to the default column
				repository.Update(context, blockNode, new PropertyBag
				                                      {
				                                      	{"column", newColumnSchema.DefaultColumn}
				                                      });
			}
		}
	}
}