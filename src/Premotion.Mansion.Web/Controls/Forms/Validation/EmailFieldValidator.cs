using System.Text.RegularExpressions;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;

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
		[Named(Constants.FormTagNamespaceUri, "emailValidator")]
		public class EmailFieldValidatorFactory : ValidationRuleFactoryTag
		{
			#region Overrides of ValidationRuleFactoryTag
			/// <summary>
			/// Create the validation rule.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="properties">The <see cref="IPropertyBag"/> of the rule properties.</param>
			/// <returns>Returns the created rule.</returns>
			protected override ValidationRule Create(MansionWebContext context, IPropertyBag properties)
			{
				return new EmailFieldValidator(properties);
			}
			#endregion
		}
		#endregion
		#region Constants
		/// <summary>
		/// RFC 2822, http://www.regular-expressions.info/email.html
		/// </summary>
		private static readonly Regex emailRegularExpression = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the validation rule.
		/// </summary>
		/// <param name="properties">The properties of this rule.</param>
		public EmailFieldValidator(IPropertyBag properties) : base(properties)
		{
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
			if (!emailRegularExpression.IsMatch(control.GetValue(context)))
				results.AddResult(this, control);
		}
		#endregion
		#region Overrides of ValidationRule
		/// <summary>
		/// Gets the default validation message of this rule.
		/// </summary>
		protected override sealed string DefaultValidationMessage
		{
			get { return "Does not contain a valid e-mail address."; }
		}
		#endregion
	}
}