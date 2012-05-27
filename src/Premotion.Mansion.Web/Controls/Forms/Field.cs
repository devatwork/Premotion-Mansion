using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Controls.Forms.Validation;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Base class for all field controls.
	/// </summary>
	public abstract class Field : FormControl
	{
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected Field(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether this field has a value.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns true when the field is considered to have value, otherwise false.</returns>
		public abstract bool HasValue(IMansionContext context);
		#endregion
	}
	/// <summary>
	/// Base class for all field controls.
	/// </summary>
	/// <typeparam name="TValue">The type of value this field contains.</typeparam>
	public abstract class Field<TValue> : Field
	{
		#region Field Factory Tag
		/// <summary>
		/// Base tag for <see cref="ScriptTag"/>s creating <see cref="Field{TValue}"/>s.
		/// </summary>
		/// <typeparam name="TField">The type of <see cref="Field{TValue}"/> created by this factory.</typeparam>
		public abstract class FieldFactoryTag<TField> : FormControlFactoryTag<TField> where TField : Field<TValue>
		{
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected Field(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of Control
		/// <summary>
		/// Initializes this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		protected override void DoInitialize(IMansionWebContext context)
		{
			base.DoInitialize(context);

			// set readonly flag
			Name = Definition.Properties.Get<string>(context, "name");
			if (string.IsNullOrEmpty(Name))
				throw new InvalidOperationException(string.Format("Name is required for fields of type {0}", GetType()));

			// set default value
			TValue defaultValue;
			if (Definition.Properties.TryGet(context, "defaultValue", out defaultValue))
			{
				HasDefaultValue = true;
				DefaultValue = defaultValue;
			}

			// set readonly flag
			IsReadOnly = Definition.Properties.Get(context, "readonly", false);
		}
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			// set the field value
			Definition.Properties.Set("Value", GetValue(context));

			// render the field);)
			using (templateService.Render(context, "FieldContainer"))
				base.DoRender(context, templateService);
		}
		#endregion
		#region Overrides of FormControl
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			OwnerForm = form;

			// if the form is posted back to the server and this control is read-only, remove the value from the field properties
			TValue newValue;
			if (IsReadOnly && form.State.IsPostback)
				form.State.FieldProperties.TryGetAndRemove(context, Name, out newValue);
				// if the value of the control is already set than do nothing
			else if (form.State.FieldProperties.TryGet(context, Name, out newValue))
				SetValue(newValue);
				// if the form has a data source try to retrieve the value of this field from it
			else if (form.State.HasDataSource && form.State.DataSource.TryGet(context, Name, out newValue))
				SetValue(newValue);
				// check if the control has a default value
			else if (HasDefaultValue)
				SetValue(DefaultValue);
		}
		/// <summary>
		/// Adds a new <paramref name="rule"/> to this control.
		/// </summary>
		/// <param name="rule">The <see cref="ValidationRule"/> which to add.</param>
		public override void Add(ValidationRule rule)
		{
			base.Add(rule);

			// check for required field
			if (rule is RequiredFieldValidator)
				Definition.Properties.Set("isRequired", true);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this field.
		/// </summary>
		protected string Name { get; private set; }
		/// <summary>
		/// Gets a flag indicating whether this field has a default value.
		/// </summary>
		protected bool HasDefaultValue { get; private set; }
		/// <summary>
		/// Gets the default value of this field.
		/// </summary>
		protected TValue DefaultValue { get; private set; }
		/// <summary>
		/// Gets a flag indicating whether this field is readonly.
		/// </summary>
		protected bool IsReadOnly { get; private set; }
		/// <summary>
		/// Gets the owner form of this control.
		/// </summary>
		private Form OwnerForm
		{
			get
			{
				if (ownerForm == null)
					throw new InvalidOperationException("The control was not initialized properly");
				return ownerForm;
			}
			set
			{
				if (ownerForm != null)
					throw new InvalidOperationException("The OwnerForm is already set");
				ownerForm = value;
			}
		}
		/// <summary>
		/// Gets the value of this field.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the value.</returns>
		public TValue GetValue(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			return OwnerForm.State.FieldProperties.Get<TValue>(context, Name);
		}
		/// <summary>
		/// Gets the value of this field.
		/// </summary>
		/// <param name="value">The value which to set.</param>
		protected void SetValue(TValue value)
		{
			OwnerForm.State.FieldProperties.Set(Name, value);
		}
		/// <summary>
		/// Clears the value of this field.
		/// </summary>
		protected void ClearValue()
		{
			OwnerForm.State.FieldProperties.Remove(Name);
		}
		/// <summary>
		/// Gets a flag indicating whether this field has a value.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns true when the field is considered to have value, otherwise false.</returns>
		public override bool HasValue(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			string stringValue;
			if (!OwnerForm.State.FieldProperties.TryGet(context, Name, out stringValue))
				return false;
			return !string.IsNullOrEmpty(stringValue);
		}
		#endregion
		#region Private Fields
		private Form ownerForm;
		#endregion
	}
}