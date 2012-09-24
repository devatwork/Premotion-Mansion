using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Validation
{
	/// <summary>
	/// Implements the e-mail address field validation rule.
	/// </summary>
	public class EmailFieldValidator : ValidationRule<Field<string>>
	{
		#region Nested type: EmailFieldValidatorFactory
		/// <summary>
		/// Constructs <see cref="EmailFieldValidator"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "emailValidator")]
		public class EmailFieldValidatorFactory : ValidationRuleFactoryTag
		{
			#region Overrides of ValidationRuleFactoryTag
			/// <summary>
			/// Create the validation rule.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="properties">The <see cref="IPropertyBag"/> of the rule properties.</param>
			/// <returns>Returns the created rule.</returns>
			protected override ValidationRule Create(IMansionWebContext context, IPropertyBag properties)
			{
				return new EmailFieldValidator(properties);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the validation rule.
		/// </summary>
		/// <param name="properties">The properties of this rule.</param>
		public EmailFieldValidator(IEnumerable<KeyValuePair<string, object>> properties) : base(properties, "Does not contain a valid e-mail address.")
		{
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
			if (!control.GetValue(context).IsValidEmailAddress())
				results.AddResult(context, GetFormattedMessage(context), control);
		}
		#endregion
	}
}