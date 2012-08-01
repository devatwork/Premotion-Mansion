using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents a column of a <see cref="Grid"/>.
	/// </summary>
	public abstract class Column : IControl
	{
		#region Nested type: ColumnFactoryTag
		/// <summary>
		/// Creates <see cref="Column"/>s.
		/// </summary>
		public abstract class ColumnFactoryTag : ScriptTag
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

				// get the grid control from the stack
				Grid grid;
				if (!webContext.TryPeekControl(out grid))
					throw new InvalidOperationException(string.Format("'{0}' can only be added to '{1}'", GetType(), typeof (Grid)));

				// create the column
				var column = Create(webContext);

				// allow potential child tags to decorate the column
				using (webContext.ControlStack.Push(column))
					ExecuteChildTags(webContext);

				// add the column to the grid
				grid.Add(column);
			}
			/// <summary>
			/// Create a <see cref="Column"/> instance.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created <see cref="Column"/>.</returns>
			protected abstract Column Create(IMansionWebContext context);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="properties">The properties of this column.</param>
		protected Column(IPropertyBag properties)
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
		/// Renders the header of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		public void RenderHeaderFilter(IMansionWebContext context, ITemplateService templateService, Dataset dataset)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (dataset == null)
				throw new ArgumentNullException("dataset");

			// invoke template method
			DoRenderHeaderFilter(context, templateService, dataset);
		}
		/// <summary>
		/// Renders the header of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		protected virtual void DoRenderHeaderFilter(IMansionWebContext context, ITemplateService templateService, Dataset dataset)
		{
			// if the column has got a filter, allow it to render the header
			if (Filter != null)
				Filter.RenderHeader(context, templateService, dataset);
			else
				templateService.Render(context, "GridControlNoFilter").Dispose();
		}
		/// <summary>
		/// Renders the header of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		public void RenderHeader(IMansionWebContext context, ITemplateService templateService, Dataset dataset)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (dataset == null)
				throw new ArgumentNullException("dataset");

			// do invoke template method
			DoRenderHeader(context, templateService, dataset);
		}
		/// <summary>
		/// Renders the header of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		protected virtual void DoRenderHeader(IMansionWebContext context, ITemplateService templateService, Dataset dataset)
		{
			using (context.Stack.Push("ColumnProperties", Properties))
			{
				// if the column has got a sort, allow it to render the header
				if (Sort != null)
					Sort.RenderHeader(context, templateService, dataset);
				else
					templateService.Render(context, "GridControl" + GetType().Name + "Header").Dispose();
			}
		}
		/// <summary>
		/// Renders a cell of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		/// <param name="row">The being rendered.</param>
		public void RenderCell(IMansionWebContext context, ITemplateService templateService, Dataset dataset, IPropertyBag row)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (dataset == null)
				throw new ArgumentNullException("dataset");
			if (row == null)
				throw new ArgumentNullException("row");

			using (context.Stack.Push("ColumnProperties", Properties))
			using (templateService.Render(context, "GridControlCell"))
				DoRenderCell(context, templateService, dataset, row);
		}
		/// <summary>
		/// Renders a cell of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		/// <param name="row">The being rendered.</param>
		protected abstract void DoRenderCell(IMansionWebContext context, ITemplateService templateService, Dataset dataset, IPropertyBag row);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the properties of this column.
		/// </summary>
		public IPropertyBag Properties { get; private set; }
		/// <summary>
		/// Gets a flag indicating whether this column has a filter.
		/// </summary>
		public bool HasFilter
		{
			get { return Filter != null; }
		}
		/// <summary>
		/// Gets the <see cref="ColumnSort"/>.
		/// </summary>
		public ColumnSort Sort { private get; set; }
		/// <summary>
		/// Gets the <see cref="ColumnFilter"/>.
		/// </summary>
		public ColumnFilter Filter { private get; set; }
		#endregion
	}
}