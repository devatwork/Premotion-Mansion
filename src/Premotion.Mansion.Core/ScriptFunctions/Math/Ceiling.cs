using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Returns the smallest integral value that is greater than or equal to the specified double-precision floating-point number.
	/// </summary>
	[ScriptFunction("Ceiling")]
	public class Ceiling : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Evaluate(IMansionContext context, double value)
		{
			return (int) System.Math.Ceiling(value);
		}
	}
}