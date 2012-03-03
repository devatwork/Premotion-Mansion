using System;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Url
{
	/// <summary>
	/// Implements <see cref="NodeUrlGenerator"/> for site nodes.
	/// </summary>
	public class SiteUrlGenerator : NodeUrlGenerator
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
			// check if the site node has a prefered host header, or pick the first hostheader
			string preferedHostheader;
			String hostheaderString;
			if (node.TryGet(context, "preferedHostheader", out preferedHostheader) && !string.IsNullOrEmpty(preferedHostheader))
				uriBuilder.Host = preferedHostheader;
			else if (node.TryGet(context, "hostheaders", out hostheaderString))
			{
				var hostheaders = hostheaderString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();

				// if there are not hostheaders set or the current hostheader is in the hostheaders dont change the host
				if (hostheaders.Count == 0 || hostheaders.Contains(uriBuilder.Host, StringComparer.OrdinalIgnoreCase))
					return;

				// set the new hostheader
				uriBuilder.Host = hostheaders[0];
			}
		}
		#endregion
	}
}