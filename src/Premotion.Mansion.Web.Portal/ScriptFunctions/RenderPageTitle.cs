using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Renders the title of this page.
	/// </summary>
	[ScriptFunction("RenderPageTitle")]
	public class RenderPageTitle : FunctionExpression
	{
		/// <summary>
		/// Renders the title of this page.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the HTML for this block.</returns>
		public string Evaluate(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the site node and the url node
			var siteNode = context.Stack.Peek<Node>("SiteNode");
			var contentNode = context.Stack.Peek<Node>("ContentNode");

			// get the title of the site
			string siteTitle;
			if (!siteNode.TryGet(context, "seoTitle", out siteTitle) || string.IsNullOrEmpty(siteTitle))
				siteTitle = siteNode.Pointer.Name;

			// if the home page is displayed, show only the title of the site node
			if (siteNode.Pointer.Id == contentNode.Pointer.Id)
				return siteTitle;

			// get the title of the site
			string pageTitle;
			if (!contentNode.TryGet(context, "seoTitle", out pageTitle) || string.IsNullOrEmpty(pageTitle))
				pageTitle = contentNode.Pointer.Name;

			// return the assembled title
			return pageTitle + " - " + siteTitle;
		}
	}
}