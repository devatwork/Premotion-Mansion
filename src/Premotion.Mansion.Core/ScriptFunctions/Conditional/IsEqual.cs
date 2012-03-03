using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether A is equal ton B.
	/// </summary>
	[ScriptFunction("IsEqual")]
	public class IsEqual : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, object left, object right)
		{
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the conversion service
			var conversionService = context.Nucleus.Get<IConversionService>(context);

			// compare the two values
			return conversionService.Compare(context, left, right) == 0;
		}
	}
}