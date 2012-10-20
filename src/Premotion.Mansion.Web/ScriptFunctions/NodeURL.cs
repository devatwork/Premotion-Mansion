using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Urls;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates a URL for the node.
	/// </summary>
	[ScriptFunction("NodeURL")]
	public class NodeUrl : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nodeUrlService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public NodeUrl(INodeUrlService nodeUrlService)
		{
			// validate arguments
			if (nodeUrlService == null)
				throw new ArgumentNullException("nodeUrlService");

			// set values
			this.nodeUrlService = nodeUrlService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Generates a URL for the node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <returns>The <see cref="Uri"/> generated for the <paramref name="node"/>.</returns>
		public Url Evaluate(IMansionContext context, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			// return the url
			return nodeUrlService.Generate(context.Cast<IMansionWebContext>(), node);
		}
		#endregion
		#region Private Fields
		private readonly INodeUrlService nodeUrlService;
		#endregion
	}
}