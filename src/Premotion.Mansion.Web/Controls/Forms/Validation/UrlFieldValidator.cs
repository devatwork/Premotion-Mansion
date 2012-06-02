using System.Collections.Generic;
using System.Text.RegularExpressions;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Validation
{
	/// <summary>
	/// Implements the e-mail address field validation rule.
	/// </summary>
	public class UrlFieldValidator : ValidationRule<Field<string>>
	{
		#region Nested type: UrlFieldValidatorFactory
		/// <summary>
		/// Constructs <see cref="EmailFieldValidator"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "urlValidator")]
		public class UrlFieldValidatorFactory : ValidationRuleFactoryTag
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
				return new UrlFieldValidator(properties);
			}
			#endregion
		}
		#endregion
		#region Constants
		/// <summary>
		/// http://flanders.co.nz/2009/11/08/a-good-url-regular-expression-repost/
		/// </summary>
		private static readonly Regex urlRegularExpression = new Regex(@"^(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~\/|\/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:\/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|\/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=?(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=?(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the validation rule.
		/// </summary>
		/// <param name="properties">The properties of this rule.</param>
		public UrlFieldValidator(IEnumerable<KeyValuePair<string, object>> properties) : base(properties, "Does not contain a valid URL.")
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
			if (!urlRegularExpression.IsMatch(control.GetValue(context)))
				results.AddResult(context, GetFormattedMessage(context), control);
		}
		#endregion
	}
}