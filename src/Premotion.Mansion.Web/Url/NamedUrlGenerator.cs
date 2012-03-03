using System;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Url
{
	/// <summary>
	/// Implements <see cref="NodeUrlGenerator"/> for named urls.
	/// </summary>
	public class NamedUrlGenerator : NodeUrlGenerator
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
			uriBuilder.Path = HttpUtilities.CombineIntoRelativeUrl(context.HttpContext.Request.ApplicationPath, string.Join("/", node.Pointer.Path.Skip(2).Select(HttpUtilities.EscapeUriString)) + "." + node.Pointer.Id + Constants.ExecutableScriptExtension);
		}
		#endregion
	}
}