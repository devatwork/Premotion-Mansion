using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls
{
	/// <summary>
	/// The base class of all web control containers. A containers contiains other <see cref="Control"/>s.
	/// </summary>
	public abstract class Container : Control
	{
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected Container(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Control Methods
		/// <summary>
		/// Adds the <paramref name="control"/> to this container.
		/// </summary>
		/// <param name="control">The <see cref="Control"/> which to add to this container.</param>
		public void Add(Control control)
		{
			// validate arguments
			if (control == null)
				throw new ArgumentNullException("control");
			controls.Add(control);
		}
		#endregion
		#region Overrides of Control
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			// loop over all the controls and render them
			using (templateService.Render(context, GetType().Name + "Control"))
			{
				foreach (var control in Controls)
					control.Render(context, templateService);
			}
		}
		/// <summary>
		/// Finds the <see cref="Control"/> with the specified <paramref name="name"/>;
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/></param>
		/// <param name="name">The ID of the control.</param>
		/// <param name="control">The <see cref="Control"/> if found.</param>
		/// <returns>Returns true if the control is found, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null or empty.</exception>
		public override bool TryFindControlByName(IMansionContext context, string name, out Control control)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// check if the id matches this container
			if (base.TryFindControlByName(context, name, out control))
				return true;

			// check the children
			foreach (var child in controls)
			{
				if (child.TryFindControlByName(context, name, out control))
					return true;
			}

			// no control found
			control = null;
			return false;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="Control"/>s in this container.
		/// </summary>
		protected IEnumerable<Control> Controls
		{
			get { return controls; }
		}
		#endregion
		#region Private Fields
		private readonly List<Control> controls = new List<Control>();
		#endregion
	}
}