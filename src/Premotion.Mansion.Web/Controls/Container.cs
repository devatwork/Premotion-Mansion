using System;
using System.Collections.Generic;
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
		#region Add Control Methods
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