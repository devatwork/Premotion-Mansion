using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates an relative url from the <see cref="Uri"/>.
	/// </summary>
	[ScriptFunction("MakeRelativeUrl"), Obsolete]
	public class MakeRelativeUrl : FunctionExpression
	{
		/// <summary>
		/// Generates an absulute url from the <see cref="Uri"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="url">The <see cref="Uri"/> which to make absolute.</param>
		/// <returns>The <see cref="Uri"/> ot the static resource.</returns>
		public Url Evaluate(IMansionContext context, Url url)
		{
			throw new NotSupportedException("No longer supported");
		}
	}
}