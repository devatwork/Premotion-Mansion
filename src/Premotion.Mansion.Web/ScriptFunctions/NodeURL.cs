using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates a URL for the node.
	/// </summary>
	[ScriptFunction("NodeURL")]
	public class NodeUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a URL for the node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <returns>The <see cref="Uri"/> generated for the <paramref name="node"/>.</returns>
		public Uri Evaluate(MansionContext context, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			// return the url
			return context.Nucleus.Get<INodeUrlService>(context).Generate(context.Cast<MansionWebContext>(), node);
		}
	}
}