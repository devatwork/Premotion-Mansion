using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Urls
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
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <param name="nodeType">The <see cref="ITypeDefinition"/> of the node.</param>
		/// <param name="url">The <see cref="Url"/> which to use to build the url.</param>
		protected override void DoGenerate(IMansionWebContext context, Node node, ITypeDefinition nodeType, Url url)
		{
			url.PathSegments = HttpUtilities.CombineIntoRelativeUrl(node.Pointer.Id + "/" + string.Join("/", node.Pointer.Path.Skip(2).Select(HttpUtilities.EscapeUriString)));
		}
		#endregion
	}
}