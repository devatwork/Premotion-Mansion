using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Forms.Validation
{
	/// <summary>
	/// Displays the messages in a <see cref="ValidationResults"/> of a particular control.
	/// </summary>
	public class ValidationSummary : FormControl
	{
		#region Nested type: ValidationSummaryFactoryTag
		/// <summary>
		/// This tags creates <see cref="Step"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "validationSummary")]
		public class ValidationSummaryFactoryTag : FormControlFactoryTag<ValidationSummary>
		{
			#region Overrides of ControlFactoryTag<Step>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override ValidationSummary Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new ValidationSummary(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public ValidationSummary(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of FormControl
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			// get the results from the form.
			var results = context.CurrentForm.ValidationResults;

			// if there are no validation messages dont render
			if (results.IsValid)
				return;

			// render the frame
			using (templateService.Render(context, GetType().Name + "Control"))
			{
				// loop over all the individual messsages
				foreach (var result in results.Results)
				{
					// push control and message properties to the stack
					using (context.Stack.Push("ValidationResult", PropertyBagAdapterFactory.Adapt(context, result)))
						templateService.Render(context, "ValidationResult").Dispose();
				}
			}
		}
		#endregion
	}
}