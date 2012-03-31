using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Validation
{
	/// <summary>
	/// Implements the required field validation rule.
	/// </summary>
	public class RequiredFieldValidator : ValidationRule<Field>
	{
		#region Nested type: RequiredFieldValidatorFactory
		/// <summary>
		/// Constructs <see cref="RequiredFieldValidator"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "requiredFieldValidator")]
		public class RequiredFieldValidatorFactory : ValidationRuleFactoryTag
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
				return new RequiredFieldValidator(properties);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the validation rule.
		/// </summary>
		/// <param name="properties">The properties of this rule.</param>
		public RequiredFieldValidator(IPropertyBag properties) : base(properties)
		{
		}
		#endregion
		#region Overrides of ValidationRule<Field>
		/// <summary>
		/// Executes the validation rule.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being validated.</param>
		/// <param name="control">The actual <see cref="FormControl"/> being validated.</param>
		/// <param name="results">The <see cref="ValidationResults"/> containing the validation results.</param>
		protected override void DoValidate(IMansionWebContext context, Form form, Field control, ValidationResults results)
		{
			// if the field has a value do nothing
			if (control.HasValue(context))
				return;

			// add the result
			results.AddResult(this, control);
		}
		#endregion
		#region Overrides of ValidationRule
		/// <summary>
		/// Gets the default validation message of this rule.
		/// </summary>
		protected override string DefaultValidationMessage
		{
			get { return "Is a required field"; }
		}
		#endregion
	}
}