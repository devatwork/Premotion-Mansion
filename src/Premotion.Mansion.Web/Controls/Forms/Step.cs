﻿using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// A step of an <see cref="Form"/>.
	/// </summary>
	public class Step : FormActionContainer
	{
		#region Nested type: StepFactoryTag
		/// <summary>
		/// This tags creates <see cref="Step"/>s.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "step")]
		public class StepFactoryTag : FormControlFactoryTag<Step>
		{
			#region Overrides of ControlFactoryTag<Step>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Step Create(MansionWebContext context, ControlDefinition definition)
			{
				return new Step(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Step(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}