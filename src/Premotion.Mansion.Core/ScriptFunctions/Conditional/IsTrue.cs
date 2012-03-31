using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether the value is true, otherwise it will return false.
	/// </summary>
	[ScriptFunction("IsTrue")]
	public class IsTrue : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="conversionService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public IsTrue(IConversionService conversionService)
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
		/// <param name="input"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, object input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// convert the value
			return conversionService.Convert(context, input, false);
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}