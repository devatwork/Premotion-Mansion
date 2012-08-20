using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Retrieves the layout node for the specified source node.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "retrieveLayoutNode")]
	public class RetrieveLayoutNodeTag : RetrieveRecordBaseTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RetrieveLayoutNodeTag(IQueryParser parser, ITypeService typeService) : base(parser)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region Overrides of RetrieveNodeBaseTag
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository">The <see cref="IRepository"/>.</param>
		/// <param name="parser">The <see cref="IQueryParser"/>.</param>
		/// <returns>Returns the result.</returns>
		protected override Record Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository, IQueryParser parser)
		{
			// get the node
			var contentNode = GetRequiredAttribute<Node>(context, "source");

			//  get the page type
			var pageType = typeService.Load(context, "Page");

			// find the closest node pointer of type Page
			var pageNodePointer = contentNode.Pointer.HierarchyReverse.FirstOrDefault(x => typeService.Load(context, x.Type).IsAssignable(pageType));
			if (pageNodePointer == null)
				throw new InvalidOperationException(string.Format("Node '{0}' does not have a parent which is a Page", contentNode.Pointer.StructureString));

			// retrieve and return the parent node
			return repository.RetrieveSingleNode(context, new PropertyBag {{"id", pageNodePointer.Id}});
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}