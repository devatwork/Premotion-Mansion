using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a time <see cref="Field{TValue}"/>.
	/// </summary>
	public class TimeField : Field<TimeSpan>
	{
		#region Nested type: TimeSpanFactoryTag
		/// <summary>
		/// This tag creates a <see cref="TimeSpan"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "time")]
		public class TimeSpanFactoryTag : FieldFactoryTag<TimeField>
		{
			#region Overrides of FieldFactoryTag<TimeSpan>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override TimeField Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new TimeField(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public TimeField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of Field
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

			// try to get the value
			var value = GetValue(context);

			// check if the value is set
			return value != TimeSpan.MinValue;
		}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			base.DoInitialize(context, form);

			// if the timespan is null, remove it from the properties
			TimeSpan time;
			var value = form.State.FieldProperties.TryGetAndRemove(context, Name, out time) && time != TimeSpan.MinValue ? (object) time : null;
			form.State.FieldProperties.Set(Name, value);
		}
		#endregion
	}
}