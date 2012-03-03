using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Implements the equals function.
	/// </summary>
	[ScriptFunction("Equal")]
	public class Equal : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="leftHand"></param>
		/// <param name="rightHand"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, string leftHand, string rightHand)
		{
			return Evaluate(context, leftHand, rightHand, true);
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="leftHand"></param>
		/// <param name="rightHand"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, string leftHand, string rightHand, bool ignoreCase)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (leftHand == null)
				return false;
			if (rightHand == null)
				return false;

			return string.Equals(leftHand, rightHand, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		}
	}
}