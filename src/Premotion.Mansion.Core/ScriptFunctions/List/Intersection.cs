using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.List
{
	/// <summary>
	/// Gets an internsaction between two CSVs.
	/// </summary>
	[ScriptFunction("Intersection")]
	public class Intersection : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="candidateA"></param>
		/// <param name="candidateB"></param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, string candidateA, string candidateB)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(candidateA))
				return string.Empty;
			if (string.IsNullOrEmpty(candidateB))
				return string.Empty;

			// split both lists
			var listA = candidateA.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
			var listB = candidateB.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

			return string.Join(",", listA.Intersect(listB, StringComparer.OrdinalIgnoreCase).ToArray());
		}
	}
}