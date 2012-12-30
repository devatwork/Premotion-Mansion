using System;
using Premotion.Mansion.Web.Hosting;
using dotless.Core.Exceptions;
using dotless.Core.Parser.Functions;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;

namespace Premotion.Mansion.Web.DotLess
{
	/// <summary>
	/// Turns a given path into a static resource path.
	/// </summary>
	public class StaticResourcePathFunction : Function
	{
		#region Overrides of Function
		/// <summary></summary>
		/// <param name="env"></param>
		/// <returns></returns>
		protected override Node Evaluate(Env env)
		{
			// validate arguments
			if (Arguments.Count != 1)
				throw new ParserException("The StaticResourcePathFunction should be invoked with exactly one parameter");

			// unescape
			Func<Node, string> unescape = n => n is Quoted ? ((Quoted) n).UnescapeContents() : n.ToCSS(env);

			// get the path
			var relativePath = unescape(Arguments[0]);

			// get the web context
			var context = DotLessContextHelper.GetContext();

			// create the url
			var url = Url.CreateUrl(context);

			// assemble the path
			url.PathSegments = WebUtilities.CombineIntoRelativeUrl(StaticResourceRequestHandler.Prefix, relativePath);
			url.CanHaveExtension = true;

			// return the result
			return new Quoted(url.ToString(), '\'', false);
		}
		#endregion
	}
}