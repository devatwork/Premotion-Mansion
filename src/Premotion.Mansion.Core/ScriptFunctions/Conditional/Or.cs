using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Combines boolean using the OR operator.
	/// </summary>
	[ScriptFunction("Or")]
	public class Or : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, bool one, bool two)
		{
			return one || two;
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <param name="three"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, bool one, bool two, bool three)
		{
			return one || two || three;
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <param name="three"></param>
		/// <param name="four"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, bool one, bool two, bool three, bool four)
		{
			return one || two || three || four;
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <param name="three"></param>
		/// <param name="four"></param>
		/// <param name="five"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, bool one, bool two, bool three, bool four, bool five)
		{
			return one || two || three || four || five;
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="one"></param>
		/// <param name="two"></param>
		/// <param name="three"></param>
		/// <param name="four"></param>
		/// <param name="five"></param>
		/// <param name="six"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, bool one, bool two, bool three, bool four, bool five, bool six)
		{
			return one || two || three || four || five || six;
		}
	}
}