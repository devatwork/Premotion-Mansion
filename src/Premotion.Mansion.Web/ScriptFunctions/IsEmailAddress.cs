using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Checks whether an given string is a valid email address.
	/// </summary>
	[ScriptFunction("IsEmailAddress")]
	public class IsEmailAddress : FunctionExpression
	{
		/// <summary>
		/// Checks whether the given <paramref name="input"/> is a valid email address.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input string.</param>
		/// <returns>Returns true if the <paramref name="input"/> is a valid emailaddress, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public bool Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check
			return input.IsValidEmailAddress();
		}
	}
}