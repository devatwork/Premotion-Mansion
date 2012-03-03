using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Renders the SEO meta tags of this page.
	/// </summary>
	[ScriptFunction("RenderSeoMetaTags")]
	public class RenderSeoMetaTags : FunctionExpression
	{
		/// <summary>
		/// Renders the title of this page.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the HTML for this block.</returns>
		public string Evaluate(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the site node and the url node
			var pageNode = context.Stack.Peek<Node>("UrlNode");

			// create the buffer in which the tags will be created
			var buffer = new StringBuilder();

			// get the application live flag
			var application = context.Stack.Peek<IPropertyBag>("Application");
			var isApplicationStaging = !application.Get(context, "live", false);

			// append the seo description when available
			string seoDescription;
			if (!pageNode.TryGet(context, "seoDescription", out seoDescription) || string.IsNullOrEmpty(seoDescription))
				seoDescription = pageNode.Get(context, "description", string.Empty).StripHtml();
			if (!string.IsNullOrEmpty(seoDescription))
			{
				buffer.Append(@"<meta name=""description"" content=""");
				buffer.Append(seoDescription.Replace("\r\n", " ").HtmlEncode());
				buffer.AppendLine(@""">");
			}

			// append the seo keywords when available
			string seoKeywords;
			if (pageNode.TryGet(context, "seoKeywords", out seoKeywords) && !string.IsNullOrEmpty(seoKeywords))
			{
				buffer.Append(@"<meta name=""keywords"" content=""");
				buffer.Append(seoKeywords.Replace("\r\n", " ").HtmlEncode());
				buffer.AppendLine(@""">");
			}

			// get the page flags
			var noIndex = isApplicationStaging || pageNode.Get(context, "seoNoIndex", false);
			var noFollow = isApplicationStaging || pageNode.Get(context, "seoNoFollow", false);
			var noCache = isApplicationStaging || pageNode.Get(context, "seoNoCache", false);

			// append the robots meta tag
			if (noIndex || noFollow)
			{
				buffer.Append(@"<meta name=""robots"" content=""");
				buffer.Append(noIndex ? "noindex" : "");
				buffer.Append((noIndex && noFollow) ? "," : "");
				buffer.Append(noFollow ? "nofollow" : "");
				buffer.AppendLine(@""">");
			}

			// append the google bot meta tag
			if (noCache)
				buffer.Append(@"<meta name=""googlebot"" content=""noarchive"">");

			// return the contents of the buffer
			return buffer.ToString();
		}
	}
}