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
			/// <param name="context">The <see cref="MansionContext"/>.</param>
			protected override void DoExecute(MansionContext context)
			{
				// get the web context
				var webContext = context.Cast<MansionWebContext>();

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
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <returns>Returns the created <see cref="Column"/>.</returns>
			protected abstract Column Create(MansionWebContext context);
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
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/> rendered in this column.</param>
		public void RenderHeaderFilter(MansionWebContext context, ITemplateService templateService, Dataset data)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (data == null)
				throw new ArgumentNullException("data");

			using (context.Stack.Push("ColumnProperties", Properties, false))
				filter.Render(context, templateService, data);
		}
		/// <summary>
		/// Renders the header of this column.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/> rendered in this column.</param>
		public void RenderHeader(MansionWebContext context, ITemplateService templateService, Dataset data)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (data == null)
				throw new ArgumentNullException("data");

			using (context.Stack.Push("ColumnProperties", Properties, false))
				sort.Render(context, templateService, data);
		}
		/// <summary>
		/// Renders a cell of this column.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/> rendered in this column.</param>
		/// <param name="row">The being rendered.</param>
		public void RenderCell(MansionWebContext context, ITemplateService templateService, Dataset data, IPropertyBag row)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (data == null)
				throw new ArgumentNullException("data");
			if (row == null)
				throw new ArgumentNullException("row");

			using (context.Stack.Push("ColumnProperties", Properties, false))
			using (templateService.Render(context, "GridControlCell"))
				DoRenderCell(context, templateService, data, row);
		}
		/// <summary>
		/// Renders a cell of this column.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/> rendered in this column.</param>
		/// <param name="row">The being rendered.</param>
		protected abstract void DoRenderCell(MansionWebContext context, ITemplateService templateService, Dataset data, IPropertyBag row);
		#endregion
		#region Column Property Methods
		/// <summary>
		/// Sets the <paramref name="columnFilter"/> to this column.
		/// </summary>
		/// <param name="columnFilter">The <see cref="ColumnFilter"/> which to set.</param>
		public void Set(ColumnFilter columnFilter)
		{
			// validate arguments
			if (columnFilter == null)
				throw new ArgumentNullException("columnFilter");
			filter = columnFilter;
		}
		/// <summary>
		/// Sets the <paramref name="columnSort"/> to this column.
		/// </summary>
		/// <param name="columnSort">The <see cref="ColumnSort"/> which to set.</param>
		public void Set(ColumnSort columnSort)
		{
			// validate arguments
			if (columnSort == null)
				throw new ArgumentNullException("columnSort");
			sort = columnSort;
		}
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
			get { return !NoColumnFilter.Instance.Equals(filter); }
		}
		#endregion
		#region Private Fields
		private ColumnFilter filter = NoColumnFilter.Instance;
		private ColumnSort sort = NoColumnSort.Instance;
		#endregion
	}
}