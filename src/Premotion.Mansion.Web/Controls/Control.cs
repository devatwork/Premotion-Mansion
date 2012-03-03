using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls
{
	/// <summary>
	/// The base class of all web controls.
	/// </summary>
	public abstract class Control : IControl
	{
		#region Nested type: ControlFactoryTag
		/// <summary>
		/// Base tag for <see cref="ScriptTag"/>s creating <see cref="Control"/>s.
		/// </summary>
		public abstract class ControlFactoryTag<TControl> : ScriptTag where TControl : Control
		{
			#region Overrides of ScriptTag
			/// <summary>
			/// Executes this tag.
			/// </summary>
			/// <param name="context">The <see cref="MansionContext"/>.</param>
			protected override void DoExecute(MansionContext context)
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

				// push the control to the stack
				using (webContext.ControlStack.Push(control))
					ExecuteChildTags(context);

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
			protected abstract TControl Create(MansionWebContext context, ControlDefinition definition);
			/// <summary>
			/// Executes the created <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="control">The control which to execute.</param>
			protected virtual void Execute(MansionWebContext context, TControl control)
			{
				// add completely initialized control to the top most control container if any otherwise process it
				Container container;
				if (context.TryFindControl(out container))
					container.Add(control);
				else
				{
					using (context.ControlStack.Push(control))
						control.Process(context);
				}
			}
			#endregion
		}
		#endregion
		#region Constants
		/// <summary>
		/// Defines the properties for creating the path to the control templates.
		/// </summary>
		public static readonly IPropertyBag ControlTemplatePathProperties = new PropertyBag
		                                                                    {
		                                                                    	{"path", "/Controls/Controls.htm"}
		                                                                    };
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected Control(ControlDefinition definition)
		{
			// validate arguments
			if (definition == null)
				throw new ArgumentNullException("definition");

			// set values
			Definition = definition;
		}
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Initializes this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		public void Initialize(MansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// store id
			DoInitialize(context);
		}
		/// <summary>
		/// Initializes this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		protected virtual void DoInitialize(MansionWebContext context)
		{
			// store id
			Definition.Properties.Set("id", Definition.Id);
		}
		#endregion
		#region Process Methods
		/// <summary>
		/// Processes this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		private void Process(MansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the services
			var applicationResourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var templateService = context.Nucleus.Get<ITemplateService>(context);

			// open the control template
			var controlTemplateResourcePath = applicationResourceService.ParsePath(context, ControlTemplatePathProperties);
			var controlTemplateResource = applicationResourceService.Get(context, controlTemplateResourcePath);
			using (templateService.Open(context, controlTemplateResource))
				DoProcess(context);
		}
		/// <summary>
		/// Processes this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		protected virtual void DoProcess(MansionWebContext context)
		{
			// render the control
			Render(context);
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		protected void Render(MansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// render the control template
			var templateService = context.Nucleus.Get<ITemplateService>(context);
			using (templateService.Render(context, "ControlContainer", Definition.Properties.Get(context, "targetField", "Control")))
			{
				// render the control
				Render(context, templateService);
			}
		}
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		public void Render(MansionWebContext context, ITemplateService templateService)
		{
			// push the control properties to the stack)
			using (context.ControlStack.Push(this))
			using (context.Stack.Push("ControlProperties", Definition.Properties, false))
			using (context.Stack.Push(GetType().Name + "Properties", Definition.Properties, false))
				DoRender(context, templateService);
		}
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected virtual void DoRender(MansionWebContext context, ITemplateService templateService)
		{
			templateService.Render(context, GetType().Name + "Control").Dispose();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the properties of this control.
		/// </summary>
		public ControlDefinition Definition { get; private set; }
		#endregion
	}
}