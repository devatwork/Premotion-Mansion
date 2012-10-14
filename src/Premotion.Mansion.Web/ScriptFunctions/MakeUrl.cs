using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates an url from a string.
	/// </summary>
	[ScriptFunction("MakeUrl")]
	public class MakeUrl : FunctionExpression
	{
		/// <summary>
		/// Generates an absulute url from the <see cref="Uri"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="relativeUrl">The <see cref="Uri"/> which to make absolute.</param>
		/// <returns>The <see cref="Url"/>.</returns>
		public Url Evaluate(IMansionContext context, string relativeUrl)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (relativeUrl == null)
				throw new ArgumentNullException("relativeUrl");

			return Url.ParseUrl(context.Cast<IMansionWebContext>(), relativeUrl);
		}
	}
}