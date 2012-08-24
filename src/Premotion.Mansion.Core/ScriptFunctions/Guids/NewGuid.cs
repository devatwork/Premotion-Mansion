using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Guids
{
	/// <summary>
	/// Returns <see cref="Guid.NewGuid"/>.
	/// </summary>
	[ScriptFunction("NewGuid")]
	public class NewGuid : FunctionExpression
	{
		/// <summary>
		/// Returns <see cref="Guid.NewGuid"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns></returns>
		public Guid Evaluate(IMansionContext context)
		{
			return Guid.NewGuid();
		}
	}
}