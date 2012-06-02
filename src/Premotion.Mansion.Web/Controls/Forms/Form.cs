using System;
using System.Collections.ObjectModel;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Controls.Forms.Engines;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// A form.
	/// </summary>
	public class Form : FormActionContainer, IFormControl
	{
		#region Nested type: FormFactoryTag
		/// <summary>
		/// Factory for <see cref="Form"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "form")]
		public class FormFactoryTag : FormControlFactoryTag<Form>
		{
			#region Overrides of ControlFactoryTag<Form>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Form Create(IMansionWebContext context, ControlDefinition definition)
			{
				// validate required attributes
				GetRequiredAttribute<string>(context, "name");

				// get the default data source
				var dataSource = definition.Properties.Get<IPropertyBag>(context, "dataSource", null);

				// create the form engine
				var formEngine = new DefaultFormEngine(dataSource);

				// create the form
				return new Form(definition, formEngine);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		/// <param name="engine">The <see cref="FormEngine"/> driving this form.</param>
		public Form(ControlDefinition definition, FormEngine engine) : base(definition)
		{
			// validate arguments
			if (engine == null)
				throw new ArgumentNullException("engine");

			// set values
			this.engine = engine;
		}
		#endregion
		#region Overrides of Control
		/// <summary>
		/// Manages the lifecycle of this form and renders itself.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		protected override void DoProcess(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// load the form state of this form
			nextStepSet = false;
			State = engine.LoadState(context, this);

			// initialize this form and its steps
			Initialize(context, this);

			using (context.Stack.Push("FormProperties", Definition.Properties, false))
			using (context.Stack.Push("FormInstanceProperties", State.FormInstanceProperties, false))
			using (context.Stack.Push("FieldProperties", State.FieldProperties, false))
			using (context.FormStack.Push(this))
			{
				// execute the prepare or post process
				if (!State.IsPostback)
				{
					// prepare this form for rendering
					ExecuteActions(context, this, State.CurrentStep);

					// store the form state
					engine.StoreState(context, this, State);
				}
				else
				{
					// validate the form and process when valid
					Validate(context, this, validationResults);

					// execute the actions
					ExecuteActions(context, this, State.CurrentStep);
					if (context.HaltExecution)
						return;

					// store the form state
					engine.StoreState(context, this, State);

					// advance the form to next step when valid
					if (validationResults.IsValid || nextStepSet)
						engine.AdvanceTo(context, this, State.NextStep);
				}

				// check if the executing should be halted
				if (context.HaltExecution)
					return;

				// set render properties
				var tmp = Steps.IndexOf(State.CurrentStep);
				Definition.Properties.Set("currentStepId", tmp);

				// render the form
				Render(context);
			}
		}
		/// <summary>
		/// Initializes this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		protected override void DoInitialize(IMansionWebContext context)
		{
			base.DoInitialize(context);

			// generate the prefixes
			Name = Definition.Properties.Get<string>(context, "name");
			Prefix = string.Format("Form-{0}-", Name);
			FieldPrefix = string.Format("Form-{0}-field-", Name);
			ActionPrefix = string.Format("Form-{0}-action-", Name);

			// store them in the control properties
			Definition.Properties.Set("prefix", Prefix);
			Definition.Properties.Set("actionPrefix", ActionPrefix);
			Definition.Properties.Set("fieldPrefix", FieldPrefix);
		}
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			// only render active step
			using (context.Stack.Push("FormControl", PropertyBagAdapterFactory.Adapt(context, this)))
			using (templateService.Render(context, GetType().Name + "Control"))
				State.CurrentStep.Render(context, templateService);
		}
		#endregion
		#region Overrides of FormControlContainer
		/// <summary>
		/// Checks whether <paramref name="controlType"/> is allowed in this container.
		/// </summary>
		/// <param name="controlType">The type of control which to check.</param>
		/// <returns>Returns true when the control is allowed, otherwise false.</returns>
		protected override bool IsControlAllowed(Type controlType)
		{
			return typeof (Step).IsAssignableFrom(controlType);
		}
		/// <summary>
		/// Validates the state of this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		/// <param name="results">The <see cref="ValidationResults"/> in which the validation results are stored.</param>
		protected override void DoValidate(IMansionWebContext context, Form form, ValidationResults results)
		{
			// loop over all the controls
			form.State.CurrentStep.Validate(context, form, results);

			// loop over all the validation rules
			foreach (var rule in ValidationRules)
				rule.Validate(context, form, this, results);
		}
		#endregion
		#region Methods
		/// <summary>
		/// Sets the next <paramref name="step"/> on this form.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="step">The <see cref="Step"/> which to set;</param>
		public void SetNextStep(IMansionWebContext context, Step step)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (step == null)
				throw new ArgumentNullException("step");
			if (!Steps.Contains(step))
				throw new ArgumentException("Can not set steps which do not belong the this form", "step");

			// check if the step is already set
			if (nextStepSet)
				throw new InvalidOperationException("The next step is already set in this form");

			// set the next step
			State.NextStep = step;
			nextStepSet = true;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this form.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the form prefix which all the controls on the form have.
		/// </summary>
		public string Prefix { get; private set; }
		/// <summary>
		/// Gets the field prefix which all the field on the form have.
		/// </summary>
		public string FieldPrefix { get; private set; }
		/// <summary>
		/// Gets the action prefix.
		/// </summary>
		public string ActionPrefix { get; private set; }
		/// <summary>
		/// Gets the <see cref="FormState"/> of this form.
		/// </summary>
		public FormState State { get; private set; }
		/// <summary>
		/// Gets the <see cref="Step"/>s of this form.
		/// </summary>
		public ReadOnlyCollection<Step> Steps
		{
			get { return new ReadOnlyCollection<Step>(FormControls.Select(control => (Step) control).ToList()); }
		}
		/// <summary>
		/// Gets the <see cref="ValidationResults"/> of this form.
		/// </summary>
		public ValidationResults ValidationResults
		{
			get { return validationResults; }
		}
		#endregion
		#region Private Fields
		private readonly FormEngine engine;
		private readonly ValidationResults validationResults = new ValidationResults();
		private bool nextStepSet;
		#endregion
	}
}