using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;

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
		public FieldValueWhitelistValidator(IPropertyBag properties, IEnumerable<string> allowedValues) : base(properties)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");
			if (allowedValues == null)
				throw new ArgumentNullException("allowedValues");

			// set values
			this.allowedValues = allowedValues.ToList();
			Properties.TrySet("message", DefaultValidationMessage);
		}
		#endregion
		#region Overrides of ValidationRule<Field<string>>
		/// <summary>
		/// Executes the validation rule.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being validated.</param>
		/// <param name="control">The actual <see cref="FormControl"/> being validated.</param>
		/// <param name="results">The <see cref="ValidationResults"/> containing the validation results.</param>
		protected override void DoValidate(MansionWebContext context, Form form, Field<string> control, ValidationResults results)
		{
			// if the field has no value do nothing
			if (!control.HasValue(context))
				return;

			// add the result
			if (!allowedValues.Contains(control.GetValue(context)))
				results.AddResult(this, control);
		}
		#endregion
		#region Overrides of ValidationRule
		/// <summary>
		/// Gets the default validation message of this rule.
		/// </summary>
		protected override sealed string DefaultValidationMessage
		{
			get { return "Does not contain a valid value."; }
		}
		#endregion
		#region Private Fields
		private readonly List<string> allowedValues;
		#endregion
	}
}