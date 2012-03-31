﻿using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a number <see cref="Field{TValue}"/>.
	/// </summary>
	public class DecimalNumber : Field<decimal>
	{
		#region Nested type: DecimalNumberTag
		/// <summary>
		/// This tag creates a <see cref="Number"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "decimalNumber")]
		public class DecimalNumberTag : FieldFactoryTag<DecimalNumber>
		{
			#region Overrides of FieldFactoryTag<Number>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override DecimalNumber Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new DecimalNumber(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public DecimalNumber(ControlDefinition definition) : base(definition)
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
			return true;
		}
		#endregion
	}
}