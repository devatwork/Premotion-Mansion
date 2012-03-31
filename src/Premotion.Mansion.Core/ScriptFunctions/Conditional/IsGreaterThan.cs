using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether A is greater than B.
	/// </summary>
	[ScriptFunction("IsGreaterThan")]
	public class IsGreaterThan : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="conversionService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public IsGreaterThan(IConversionService conversionService)
		{
			// validate arguments
			if (conversionService == null)
				throw new ArgumentNullException("conversionService");

			// set values
			this.conversionService = conversionService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, object left, object right)
		{
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// compare the two values
			return conversionService.Compare(context, left, right) > 0;
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}