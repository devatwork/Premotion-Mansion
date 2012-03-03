using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Retrieves the layout node for the specified source node.
	/// </summary>
	[Named(Constants.TagNamespaceUri, "retrieveLayoutNode")]
	public class RetrieveLayoutNodeTag : RetrieveNodeBaseTag
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

			//  get the type service
			var typeService = context.Nucleus.Get<ITypeService>(context);

			//  get the page type
			var pageType = typeService.Load(context, "Page");

			// find the closest node pointer of type Page
			var pageNodePointer = contentNode.Pointer.HierarchyReverse.FirstOrDefault(x => typeService.Load(context, x.Type).IsAssignable(pageType));
			if (pageNodePointer == null)
				throw new InvalidOperationException(string.Format("Node '{0}' does not have a parent which is a Page", contentNode.Pointer.StructureString));

			// retrieve and return the parent node
			return repository.RetrieveSingle(context, new PropertyBag {{"id", pageNodePointer.Id}});
		}
	}
}