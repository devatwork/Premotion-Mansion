using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Urls
{
	/// <summary>
	/// Generates URL's for specific node types.
	/// </summary>
	public abstract class NodeUrlGenerator
	{
		#region Generate Methods
		/// <summary>
		/// Generates an URL for the <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <param name="nodeType">The <see cref="ITypeDefinition"/> of the node.</param>
		/// <param name="url">The <see cref="Url"/> which to use to build the url.</param>
		public void Generate(IMansionWebContext context, Node node, ITypeDefinition nodeType, Url url)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");
			if (nodeType == null)
				throw new ArgumentNullException("nodeType");
			if (url == null)
				throw new ArgumentNullException("url");

			// invoke template method
			DoGenerate(context, node, nodeType, url);
		}
		/// <summary>
		/// Generates an URL for the <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <param name="nodeType">The <see cref="ITypeDefinition"/> of the node.</param>
		/// <param name="url">The <see cref="Url"/> which to use to build the url.</param>
		protected abstract void DoGenerate(IMansionWebContext context, Node node, ITypeDefinition nodeType, Url url);
		#endregion
	}
}