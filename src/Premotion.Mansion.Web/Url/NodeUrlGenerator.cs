using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Url
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
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <param name="nodeType">The <see cref="ITypeDefinition"/> of the node.</param>
		/// <param name="uriBuilder">The <see cref="UriBuilder"/> which to use to build the url.</param>
		public void Generate(MansionWebContext context, Node node, ITypeDefinition nodeType, UriBuilder uriBuilder)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");
			if (nodeType == null)
				throw new ArgumentNullException("nodeType");
			if (uriBuilder == null)
				throw new ArgumentNullException("uriBuilder");

			// invoke template method
			DoGenerate(context, node, nodeType, uriBuilder);
		}
		/// <summary>
		/// Generates an URL for the <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <param name="nodeType">The <see cref="ITypeDefinition"/> of the node.</param>
		/// <param name="uriBuilder">The <see cref="UriBuilder"/> which to use to build the url.</param>
		protected abstract void DoGenerate(MansionWebContext context, Node node, ITypeDefinition nodeType, UriBuilder uriBuilder);
		#endregion
	}
}