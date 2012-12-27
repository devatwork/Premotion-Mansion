using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Adds the second number to the first.
	/// </summary>
	[ScriptFunction("Add")]
	public class Add : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public double Evaluate(IMansionContext context, params double[] values)
		{
			// validate arguments
			if (values == null)
				throw new ArgumentNullException("values");

			return values.Aggregate(0d, (cur, res) => cur + res);
		}
	}
}