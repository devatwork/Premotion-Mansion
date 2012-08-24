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
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			protected override void DoExecute(IMansionContext context)
			{
				// get the mansion web context
				var webContext = context.Cast<IMansionWebContext>();

				// get the id of this control
				string id;
				if (!TryGetAttribute(context, "id", out id))
					id = webContext.GetNextControlId();

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
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected abstract TControl Create(IMansionWebContext context, ControlDefinition definition);
			/// <summary>
			/// Executes the created <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="control">The control which to execute.</param>
			protected virtual void Execute(IMansionWebContext context, TControl control)
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
		                                                                    	{"path", "/Controls/Controls.tpl"}
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
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		public void Initialize(IMansionWebContext context)
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
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		protected virtual void DoInitialize(IMansionWebContext context)
		{
			// store id
			Definition.Properties.Set("id", Definition.Id);
		}
		#endregion
		#region Process Methods
		/// <summary>
		/// Processes this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		private void Process(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the services
			var applicationResourceService = context.Nucleus.ResolveSingle<IApplicationResourceService>();
			var templateService = context.Nucleus.ResolveSingle<ITemplateService>();

			// open the control template
			var controlTemplateResourcePath = applicationResourceService.ParsePath(context, ControlTemplatePathProperties);
			var controlTemplateResource = applicationResourceService.Get(context, controlTemplateResourcePath);
			using (templateService.Open(context, controlTemplateResource))
				DoProcess(context);
		}
		/// <summary>
		/// Processes this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		protected virtual void DoProcess(IMansionWebContext context)
		{
			// render the control
			Render(context);
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		protected void Render(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// render the control template
			var templateService = context.Nucleus.ResolveSingle<ITemplateService>();
			using (templateService.Render(context, "ControlContainer", Definition.Properties.Get(context, "targetField", "Control")))
			{
				// render the control
				Render(context, templateService);
			}
		}
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		public void Render(IMansionWebContext context, ITemplateService templateService)
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
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected virtual void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			templateService.Render(context, GetType().Name + "Control").Dispose();
		}
		#endregion
		#region Find Methods
		/// <summary>
		/// Finds the <see cref="Control"/> with the specified <paramref name="name"/>;
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/></param>
		/// <param name="name">The ID of the control.</param>
		/// <param name="control">The <see cref="Control"/> if found.</param>
		/// <returns>Returns true if the control is found, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null or empty.</exception>
		public virtual bool TryFindControlByName(IMansionContext context, string name, out Control control)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// get the name of this control
			var controlName = Definition.Properties.Get(context, "name", string.Empty);

			// check of the ID matches this control
			if (name.Equals(controlName, StringComparison.OrdinalIgnoreCase))
			{
				control = this;
				return true;
			}

			// control not found
			control = null;
			return false;
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