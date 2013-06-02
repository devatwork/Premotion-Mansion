using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Clips the input string to the specified number of chararcters.
	/// </summary>
	[ScriptFunction("Clip")]
	public class Clip : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input, int length)
		{
			return Evaluate(context, input, length, @" &hellip;");
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <param name="length"></param>
		/// <param name="clipSymbol"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input, int length, string clipSymbol)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				return null;
			if (length < 0)
				throw new ArgumentNullException("length");
			if (clipSymbol == null)
				throw new ArgumentNullException("clipSymbol");

			// check if no clip is needed
			if (input.Length < length)
				return input;

			return input.Substring(0, length) + clipSymbol;
		}
	}
}