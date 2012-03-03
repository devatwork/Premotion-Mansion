using System;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.Web.Portal.Url
{
	/// <summary>
	/// Implements <see cref="NodeUrlGenerator"/> for content nodes.
	/// </summary>
	public class ContentUrlGenerator : NodeUrlGenerator
	{
		#region Generate Methods
		/// <summary>
		/// Generates an URL for the <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <param name="nodeType">The <see cref="ITypeDefinition"/> of the node.</param>
		/// <param name="uriBuilder">The <see cref="UriBuilder"/> which to use to build the url.</param>
		protected override void DoGenerate(MansionWebContext context, Node node, ITypeDefinition nodeType, UriBuilder uriBuilder)
		{
			// get the site node from the stack
			Node siteNode;
			if (!context.Stack.TryPeek("SiteNode", out siteNode))
				throw new InvalidOperationException("Could not find sitenode on the stack, please check application configuration.");

			// resolve the template page for this node
			var portalService = context.Nucleus.Get<IPortalService>(context);
			Node templatePageNode;
			if (!portalService.TryResolveTemplatePage(context, siteNode, node, out templatePageNode))
				throw new InvalidOperationException(string.Format("Could not find template page for node {0} ({1})", node.Pointer.PathString, node.Pointer.PointerString));

			// construct the path
			var sitePathParts = templatePageNode.Pointer.Hierarchy.SkipWhile(candidate => !siteNode.Pointer.IsParentOf(candidate)).TakeWhile(candidate => candidate.Id != templatePageNode.Pointer.Id).ToArray();
			var contentPathParts = node.Pointer.Hierarchy.Skip(2).ToArray();

			uriBuilder.Path = HttpUtilities.CombineIntoRelativeUrl(context.HttpContext.Request.ApplicationPath, string.Join("/", sitePathParts.Select(pointer => HttpUtilities.EscapeUriString(pointer.Name))), string.Join("/", contentPathParts.Select(pointer => HttpUtilities.EscapeUriString(pointer.Name))) + "." + node.Pointer.Id + Mansion.Web.Constants.ExecutableScriptExtension);
			uriBuilder.Query = string.Empty;
			uriBuilder.Fragment = string.Empty;
		}
		#endregion
	}
}