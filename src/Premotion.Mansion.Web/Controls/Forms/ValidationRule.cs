using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Base class for validation rules.
	/// </summary>
	public abstract class ValidationRule
	{
		#region Nested type: ValidationRuleFactoryTag
		/// <summary>
		/// Base <see cref="ScriptTag"/> for validation rule factories.
		/// </summary>
		public abstract class ValidationRuleFactoryTag : ScriptTag
		{
			#region Overrides of ScriptTag
			/// <summary>
			/// Executes this tag.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			protected override void DoExecute(IMansionContext context)
			{
				// get the mansion web context
				var webContext = context.Cast<IMansionWebContext>();

				// get the properties of this control
				var properties = GetAttributes(context);

				// create the rule
				var rule = Create(webContext, properties);

				// add id to the control
				FormControl validatableControl;
				if (!webContext.TryPeekControl(out validatableControl))
					throw new InvalidOperationException(string.Format("Validation rule '{0}' can only be applied to types inheriting from '{1}'", GetType(), typeof (FormControl)));

				// add the rule
				validatableControl.Add(rule);
			}
			/// <summary>
			/// Create the validation rule.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="properties">The <see cref="IPropertyBag"/> of the rule properties.</param>
			/// <returns>Returns the created rule.</returns>
			protected abstract ValidationRule Create(IMansionWebContext context, IPropertyBag properties);
			#endregion
		}
		#endregion
		#region Constuctors
		/// <summary>
		/// Constructs the validation rule.
		/// </summary>
		/// <param name="properties">The properties of this rule.</param>
		/// <param name="defaultMessage">The default validation message.</param>
		protected ValidationRule(IEnumerable<KeyValuePair<string, object>> properties, string defaultMessage)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");
			if (string.IsNullOrEmpty(defaultMessage))
				throw new ArgumentNullException("defaultMessage");

			// create a new property bag
			this.properties = new PropertyBag
			                  {
			                  	{"message", defaultMessage}
			                  };

			// merge the other properties
			this.properties.Merge(properties);
		}
		#endregion
		#region Validation Methods
		/// <summary>
		/// Executes the validation rule.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being validated.</param>
		/// <param name="control">The actual <see cref="FormControl"/> being validated.</param>
		/// <param name="results">The <see cref="ValidationResults"/> containing the validation results.</param>
		public void Validate(IMansionWebContext context, Form form, FormControl control, ValidationResults results)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (form == null)
				throw new ArgumentNullException("form");
			if (control == null)
				throw new ArgumentNullException("control");
			if (results == null)
				throw new ArgumentNullException("results");

			// invoke template method
			DoValidate(context, form, control, results);
		}
		/// <summary>
		/// Executes the validation rule.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being validated.</param>
		/// <param name="control">The actual <see cref="FormControl"/> being validated.</param>
		/// <param name="results">The <see cref="ValidationResults"/> containing the validation results.</param>
		protected abstract void DoValidate(IMansionWebContext context, Form form, FormControl control, ValidationResults results);
		#endregion
		#region Message Methods
		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the validation error message.</returns>
		protected string GetFormattedMessage(IMansionContext context)
		{
			return properties.Get<string>(context, "message");
		}
		#endregion
		#region Private Fields
		private readonly IPropertyBag properties;
		#endregion
	}
	/// <summary>
	/// Base class for all typed <see cref="ValidationRule"/>.
	/// </summary>
	/// <typeparam name="TControl">The type of <see cref="FormControl"/> which is validated by this rule.</typeparam>
	public abstract class ValidationRule<TControl> : ValidationRule where TControl : FormControl
	{
		#region Constructors
		/// <summary>
		/// Constructs the validation rule.
		/// </summary>
		/// <param name="properties">The properties of this rule.</param>
		/// <param name="defaultMessage">The default validation message.</param>
		protected ValidationRule(IEnumerable<KeyValuePair<string, object>> properties, string defaultMessage) : base(properties, defaultMessage)
		{
		}
		#endregion
		#region Overrides of ValidationRule
		/// <summary>
		/// Executes the validation rule.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being validated.</param>
		/// <param name="control">The actual <see cref="FormControl"/> being validated.</param>
		/// <param name="results">The <see cref="ValidationResults"/> containing the validation results.</param>
		protected override sealed void DoValidate(IMansionWebContext context, Form form, FormControl control, ValidationResults results)
		{
			// check type
			var typedControl = control as TControl;
			if (typedControl == null)
				throw new InvalidOperationException(string.Format("Rules of type '{0}' can only validate controls of type '{1}' and not of type '{2}'", GetType(), typeof (TControl), control.GetType()));

			// perform validation
			DoValidate(context, form, typedControl, results);
		}
		/// <summary>
		/// Executes the validation rule.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being validated.</param>
		/// <param name="control">The actual <see cref="FormControl"/> being validated.</param>
		/// <param name="results">The <see cref="ValidationResults"/> containing the validation results.</param>
		protected abstract void DoValidate(IMansionWebContext context, Form form, TControl control, ValidationResults results);
		#endregion
	}
}