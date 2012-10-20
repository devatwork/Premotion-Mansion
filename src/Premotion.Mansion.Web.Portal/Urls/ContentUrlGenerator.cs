using System;
using System.Globalization;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;
using Premotion.Mansion.Web.Urls;

namespace Premotion.Mansion.Web.Portal.Urls
{
	/// <summary>
	/// Implements <see cref="NodeUrlGenerator"/> for content nodes.
	/// </summary>
	public class ContentUrlGenerator : NodeUrlGenerator
	{
		#region Constructors
		/// <summary>
		///
		/// </summary>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ContentUrlGenerator(IPortalService portalService)
		{
			// validate arguments
			if (portalService == null)
				throw new ArgumentNullException("portalService");

			// set values
			this.portalService = portalService;
		}
		#endregion
		#region Generate Methods
		/// <summary>
		/// Generates an URL for the <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <param name="nodeType">The <see cref="ITypeDefinition"/> of the node.</param>
		/// <param name="url">The <see cref="UriBuilder"/> which to use to build the url.</param>
		protected override void DoGenerate(IMansionWebContext context, Node node, ITypeDefinition nodeType, Url url)
		{
			// get the site node from the stack
			Node siteNode;
			if (!context.Stack.TryPeek("SiteNode", out siteNode))
				throw new InvalidOperationException("Could not find sitenode on the stack, please check application configuration.");

			// resolve the template page for this node
			Node templatePageNode;
			if (!portalService.TryResolveTemplatePage(context, siteNode, node, out templatePageNode))
				throw new InvalidOperationException(string.Format("Could not find template page for node {0} ({1})", node.Pointer.PathString, node.Pointer.PointerString));

			// construct the path
			var sitePathParts = templatePageNode.Pointer.Hierarchy.SkipWhile(candidate => !siteNode.Pointer.IsParentOf(candidate)).TakeWhile(candidate => candidate.Id != templatePageNode.Pointer.Id).ToArray();
			var contentPathParts = node.Pointer.Hierarchy.Skip(2).ToArray();

			url.PathSegments = new[] {node.Pointer.Id.ToString(CultureInfo.InvariantCulture)}.Concat(sitePathParts.Select(pointer => pointer.Name)).Concat(contentPathParts.Select(pointer => pointer.Name)).ToArray();
		}
		#endregion
		#region Private Fields
		private readonly IPortalService portalService;
		#endregion
	}
}