using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a tag textbox <see cref="Field{TValue}"/>.
	/// </summary>
	public class TagTextbox : Field<string>
	{
		#region Nested type: TagTextboxFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "tagTextbox")]
		public class TagTextboxFactoryTag : FieldFactoryTag<TagTextbox>
		{
			#region Overrides of FieldFactoryTag<TagTextbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override TagTextbox Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new TagTextbox(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public TagTextbox(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of Field{TValue}
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			// retrieve tag index node
			var tagIndexNode = TagUtilities.RetrieveTagIndexNode(context);

			// push the tag index node to the stack and start rendering
			using (context.Stack.Push("TagIndexNode", tagIndexNode))
				base.DoRender(context, templateService);
		}
		#endregion
	}
}