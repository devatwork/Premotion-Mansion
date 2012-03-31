using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Converts the object to a string.
	/// </summary>
	[ScriptFunction("ConvertToString")]
	public class ConvertToString : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="conversionService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ConvertToString(IConversionService conversionService)
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
		public string Evaluate(IMansionContext context, object input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				return string.Empty;

			// convert the object to string
			return conversionService.Convert(context, input, string.Empty);
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}