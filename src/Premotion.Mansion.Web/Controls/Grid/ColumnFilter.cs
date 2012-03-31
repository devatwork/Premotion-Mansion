using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents the filter of an <see cref="Column"/>.
	/// </summary>
	public abstract class ColumnFilter : IControl
	{
		#region Nested type: ColumnFilterFactoryTag
		/// <summary>
		/// Base class for <see cref="ColumnFilter"/> factories.
		/// </summary>
		public abstract class ColumnFilterFactoryTag : ScriptTag
		{
			#region Overrides of ScriptTag
			/// <summary>
			/// Executes this tag.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			protected override void DoExecute(IMansionContext context)
			{
				// get the web context
				var webContext = context.Cast<IMansionWebContext>();

				// get the column
				Column column;
				if (!webContext.TryPeekControl(out column))
					throw new InvalidOperationException(string.Format("'{0}' must be added to a '{1}'", GetType(), typeof (Column)));

				// retrieve the properties of the filter
				var properties = GetAttributes(webContext);

				// create the filter
				var filter = Create(context, column, properties);

				// execute children
				using (webContext.ControlStack.Push(filter))
					ExecuteChildTags(context);

				// set the filter to the column
				column.Set(filter);
			}
			/// <summary>
			/// Creates a <see cref="ColumnFilter"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="column">The <see cref="Column"/> to which this filter is applied</param>
			/// <param name="properties">The properties of the filter.</param>
			/// <returns>Returns the created <see cref="ColumnFilter"/>.</returns>
			protected abstract ColumnFilter Create(IMansionContext context, Column column, IPropertyBag properties);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column filter.
		/// </summary>
		/// <param name="properties">The properties of this filter.</param>
		protected ColumnFilter(IPropertyBag properties)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");

			// set values
			Properties = properties;
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Renders this column sort.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/>.</param>
		public void Render(IMansionWebContext context, ITemplateService templateService, Dataset data)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (data == null)
				throw new ArgumentNullException("data");

			using (context.Stack.Push("ColumnFilterProperties", Properties, false))
				DoRender(context, templateService, data);
		}
		/// <summary>
		/// Renders this column sort.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/>.</param>
		protected abstract void DoRender(IMansionWebContext context, ITemplateService templateService, Dataset data);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the properties of this filter.
		/// </summary>
		protected IPropertyBag Properties { get; private set; }
		#endregion
		#region Implementation of IControl
		/// <summary>
		/// Gets the <see cref="ControlDefinition"/> of this form control.
		/// </summary>
		public ControlDefinition Definition
		{
			get { throw new NotImplementedException(); }
		}
		#endregion
	}
}