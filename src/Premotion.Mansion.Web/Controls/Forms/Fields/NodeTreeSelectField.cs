using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Represents a control which allows to select a node from a tree.
	/// </summary>
	public class NodeTreeSelectField : TreeField<string>
	{
		#region Nested type: NodeTreeSelectFactoryTag
		/// <summary>
		/// Constructs <see cref="NodeTreeSelectField"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "nodeTreeSelect")]
		public class NodeTreeSelectFactoryTag : TreeFieldFactoryTag<NodeTreeSelectField>
		{
			#region Overrides of TreeFieldFactoryTag<NodeTreeSelect>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override NodeTreeSelectField DoCreate(IMansionWebContext context, ControlDefinition definition)
			{
				return new NodeTreeSelectField(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public NodeTreeSelectField(ControlDefinition definition) : base(definition)
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