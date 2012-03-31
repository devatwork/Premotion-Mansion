using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether the value is false, otherwise it will return true.
	/// </summary>
	[ScriptFunction("IsFalse")]
	public class IsFalse : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="conversionService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public IsFalse(IConversionService conversionService)
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
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// convert the value
			return conversionService.Convert(context, input, true);
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}