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
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns></returns>
		public Guid Evaluate(MansionContext context)
		{
			return Guid.Empty;
		}
	}
}