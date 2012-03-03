using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether 
	/// </summary>
	[ScriptFunction("StartsWith")]
	public class StartsWith : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="haystack"></param>
		/// <param name="needle"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, string haystack, string needle)
		{
			// valdidate arguments
			if (haystack == null)
				throw new ArgumentNullException("haystack");
			if (needle == null)
				throw new ArgumentNullException("needle");

			return haystack.StartsWith(needle, StringComparison.OrdinalIgnoreCase);
		}
	}
}