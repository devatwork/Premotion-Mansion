using System;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Urls
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
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <param name="nodeType">The <see cref="ITypeDefinition"/> of the node.</param>
		/// <param name="url">The <see cref="Url"/> which to use to build the url.</param>
		protected override void DoGenerate(IMansionWebContext context, Node node, ITypeDefinition nodeType, Url url)
		{
			// check if the site node has a prefered host header, or pick the first hostheader
			string preferedHostheader;
			String hostheaderString;
			if (node.TryGet(context, "preferedHostheader", out preferedHostheader) && !string.IsNullOrEmpty(preferedHostheader))
				url.HostName = preferedHostheader;
			else if (node.TryGet(context, "hostheaders", out hostheaderString))
			{
				var hostheaders = hostheaderString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();

				// if there are not hostheaders set or the current hostheader is in the hostheaders dont change the host
				if (hostheaders.Count == 0 || hostheaders.Contains(url.HostName, StringComparer.OrdinalIgnoreCase))
					return;

				// set the new hostheader
				url.HostName = hostheaders[0];
			}
		}
		#endregion
	}
}