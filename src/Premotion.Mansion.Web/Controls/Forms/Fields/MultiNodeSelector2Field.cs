using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// A node selector interface.
	/// </summary>
	public class MultiNodeSelector2Field : Field<string>
	{
		#region Nested type: MultiNodeSelector2FieldFactoryTag
		/// <summary>
		/// Constructs <see cref="NodeTreeSelectField"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "multiNodeSelector2")]
		public class MultiNodeSelector2FieldFactoryTag : FieldFactoryTag<MultiNodeSelector2Field>
		{
			#region Overrides of FieldFactoryTag<SingleNodeSelector2Field>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override MultiNodeSelector2Field Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new MultiNodeSelector2Field(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a field.
		/// </summary>
		/// <param name="definition"></param>
		public MultiNodeSelector2Field(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}