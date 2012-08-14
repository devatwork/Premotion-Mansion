using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Retrieves the template page for the specified source node.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "retrieveTemplatePageNode")]
	public class RetrieveTemplatePageNodeTag : RetrieveRecordBaseTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RetrieveTemplatePageNodeTag(IQueryParser parser, IPortalService portalService) : base(parser)
		{
			// validate arguments
			if (portalService == null)
				throw new ArgumentNullException("portalService");

			// set values
			this.portalService = portalService;
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
		protected override IPropertyBag Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository, IQueryParser parser)
		{
			// get the node
			var contentNode = GetRequiredAttribute<Node>(context, "source");
			var siteNode = GetRequiredAttribute<Node>(context, "siteNode");

			// resolve the template page node
			Node templatePageNode;
			portalService.TryResolveTemplatePage(context, siteNode, contentNode, out templatePageNode);
			return templatePageNode;
		}
		#endregion
		#region Private Fields
		private readonly IPortalService portalService;
		#endregion
	}
}