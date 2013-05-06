using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Math
{
	/// <summary>
	/// Returns the largest integer less than or equal to the specified double-precision floating-point number.
	/// </summary>
	[ScriptFunction("Floor")]
	public class Floor : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Evaluate(IMansionContext context, double value)
		{
			return (int) System.Math.Floor(value);
		}
	}
}