﻿using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a multi select <see cref="ListField{TValue}"/>.
	/// </summary>
	public class MultiSelectField : ListField<string>
	{
		#region Nested type: MultiSelectFactoryTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "multiselect")]
		public class MultiSelectFactoryTag : ListFieldFactoryTag<MultiSelectField>
		{
			#region Overrides of FieldFactoryTag<MultiSelect>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			/// <param name="mappingStrategy">The <see cref="RowMappingStrategy"/> used by the list field.</param>
			protected override MultiSelectField Create(IMansionWebContext context, ControlDefinition definition, RowMappingStrategy mappingStrategy)
			{
				return new MultiSelectField(definition, mappingStrategy);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		/// <param name="mappingStrategy">The <see cref="RowMappingStrategy"/> used by the list field.</param>
		public MultiSelectField(ControlDefinition definition, RowMappingStrategy mappingStrategy) : base(definition, mappingStrategy)
		{
		}
		#endregion
		#region Overrides of Field
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			// if the form is posted back make sure the boolean is set when the checkbox was unchecked
			if (form.State.IsPostback)
				form.State.FieldProperties.Set(Name, form.State.FieldProperties.Get(context, Name, string.Empty));

			base.DoInitialize(context, form);
		}
		#endregion
	}
}