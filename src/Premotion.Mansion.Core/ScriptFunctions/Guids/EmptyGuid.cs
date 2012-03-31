using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Guids
{
	/// <summary>
	/// Returns <see cref="Guid.Empty"/>.
	/// </summary>
	[ScriptFunction("EmptyGuid")]
	public class EmptyGuid : FunctionExpression
	{
		/// <summary>
		/// Returns <see cref="Guid.Empty"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns></returns>
		public Guid Evaluate(IMansionContext context)
		{
			return Guid.Empty;
		}
	}
}