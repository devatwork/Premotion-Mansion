using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Concatenates the input into one long string.
	/// </summary>
	[ScriptFunction("Concat")]
	public class Concat : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, string one, string two)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return one + two;
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <param name="three"></param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, string one, string two, string three)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return one + two + three;
		}
	}
}