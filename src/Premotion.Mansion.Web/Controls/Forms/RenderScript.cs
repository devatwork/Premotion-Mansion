using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Content of this tag will only be parsed during the render phase.
	/// </summary>
	public class RenderScript : FormControl
	{
		#region Nested type: RenderScriptFactoryTag
		/// <summary>
		/// This tags creates <see cref="RenderScript"/>s.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "renderScript")]
		public class RenderScriptFactoryTag : FormControlFactoryTag<RenderScript>
		{
			#region Overrides of ControlFactoryTag<RenderScript>
			/// <summary>
			/// Executes this tag.
			/// </summary>
			/// <param name="context">The <see cref="MansionContext"/>.</param>
			protected override void DoExecute(Core.MansionContext context)
			{
				// get the mansion web context
				var webContext = context.Cast<MansionWebContext>();

				// get the id of this control
				var id = webContext.GetNextControlId();

				// get the properties of this control
				var properties = GetAttributes(context);

				// create the control definition
				var definition = new ControlDefinition(id, properties);

				// create the control
				var control = Create(webContext, definition);

				// initialize the control
				control.Initialize(webContext);

				// execute the intialized control
				Execute(webContext, control);
			}
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override RenderScript Create(MansionWebContext context, ControlDefinition definition)
			{
				return new RenderScript(definition, ExecuteChildTags);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		/// <param name="executeChildrenCallback"></param>
		public RenderScript(ControlDefinition definition, Action<MansionContext> executeChildrenCallback) : base(definition)
		{
			//  validate arguments
			if (executeChildrenCallback == null)
				throw new ArgumentNullException("executeChildrenCallback");

			// set values
			this.executeChildrenCallback = executeChildrenCallback;
		}
		#endregion
		#region Overrides of Control
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(MansionWebContext context, ITemplateService templateService)
		{
			// loop over all the controls and render them
			using (templateService.Render(context, GetType().Name + "Control"))
				executeChildrenCallback(context);
		}
		#endregion
		#region Private Fields
		private readonly Action<MansionContext> executeChildrenCallback;
		#endregion
	}
}