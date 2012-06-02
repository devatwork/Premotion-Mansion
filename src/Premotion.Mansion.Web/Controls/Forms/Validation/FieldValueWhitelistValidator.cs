using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Web.Controls.Forms.Validation
{
	/// <summary>
	/// Implements a validation rule which checks whether the value of an field is the in the supplied list of values.
	/// </summary>
	public class FieldValueWhitelistValidator : ValidationRule<Field<string>>
	{
		#region Constructors
		/// <summary>
		/// Constructs the validation rule.
		/// </summary>
		/// <param name="properties">The properties of this rule.</param>
		/// <param name="allowedValues">The allowed values of this field.</param>
		public FieldValueWhitelistValidator(IEnumerable<KeyValuePair<string, object>> properties, IEnumerable<string> allowedValues) : base(properties, "Does not contain a valid value.")
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");
			if (allowedValues == null)
				throw new ArgumentNullException("allowedValues");

			// set values
			this.allowedValues = allowedValues.ToList();
		}
		#endregion
		#region Overrides of ValidationRule<Field<string>>
		/// <summary>
		/// Executes the validation rule.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being validated.</param>
		/// <param name="control">The actual <see cref="FormControl"/> being validated.</param>
		/// <param name="results">The <see cref="ValidationResults"/> containing the validation results.</param>
		protected override void DoValidate(IMansionWebContext context, Form form, Field<string> control, ValidationResults results)
		{
			// if the field has no value do nothing
			if (!control.HasValue(context))
				return;

			// add the result
			if (!allowedValues.Contains(control.GetValue(context)))
				results.AddResult(context, GetFormattedMessage(context), control);
		}
		#endregion
		#region Private Fields
		private readonly List<string> allowedValues;
		#endregion
	}
}