using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Controls.Forms.ScriptFunctions
{
	/// <summary>
	/// Checks whether a field contains a validation error.
	/// </summary>
	[ScriptFunction("IsInValidationError")]
	public class IsInValidationError : FunctionExpression
	{
		/// <summary>
		/// Checks whether the <paramref name="field"/> contains a validation error.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="field">The <see cref="Field"/>.</param>
		public bool Evaluate(IMansionContext context, Form form, Field field)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (form == null)
				throw new ArgumentNullException("form");
			if (field == null)
				throw new ArgumentNullException("field");

			// get the name of the field
			var controlName = field.Definition.Properties.Get<string>(context, "name");

			// check if the control name occures in any of the validation messages
			return form.ValidationResults.Results.Any(candidate => controlName.Equals(candidate.ControlName, StringComparison.OrdinalIgnoreCase));
		}
	}
}