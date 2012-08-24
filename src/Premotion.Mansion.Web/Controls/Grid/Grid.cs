using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents a grid.
	/// </summary>
	public abstract class Grid : DataboundControl<Dataset>
	{
		#region Nested type: EmbeddedGrid
		/// <summary>
		/// Represents an embedded grid.
		/// </summary>
		private class EmbeddedGrid : Grid
		{
			#region Constructors
			/// <summary>
			/// Constructs a control with the specified ID.
			/// </summary>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			public EmbeddedGrid(ControlDefinition definition) : base(definition)
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: FormGrid
		/// <summary>
		/// Represents a grid with a form.
		/// </summary>
		private class FormGrid : Grid, IFormControl
		{
			#region Constructors
			/// <summary>
			/// Constructs a control with the specified ID.
			/// </summary>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			public FormGrid(ControlDefinition definition) : base(definition)
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: GridFactoryTag
		/// <summary>
		/// Constructs <see cref="Grid"/>s.
		/// </summary>
		[ScriptTag(Constants.ControlTagNamespaceUri, "grid")]
		public class GridFactoryTag : ControlFactoryTag<Grid>
		{
			#region Overrides of ControlFactoryTag<Grid>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Grid Create(IMansionWebContext context, ControlDefinition definition)
			{
				IFormControl control;
				return context.TryFindControl(out control) ? new EmbeddedGrid(definition) : (Grid) new FormGrid(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="definition"></param>
		protected Grid(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Column Methods
		/// <summary>
		/// Adds a <paramref name="column"/> to this grid.
		/// </summary>
		/// <param name="column">The <see cref="Column"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="column"/> is null.</exception>
		public void Add(Column column)
		{
			// validate arguments
			if (column == null)
				throw new ArgumentNullException("column");

			columns.Add(column);
			Definition.Properties.Set("columnCount", columns.Count);
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
			// retrieve the data
			var dataset = Retrieve(context);

			// render the control
			using (context.Stack.Push("GridProperties", Definition.Properties))
			using (context.Stack.Push("Dataset", dataset))
			using (templateService.Render(context, GetType().Name + "Control"))
			{
				// render the headers per column
				using (templateService.Render(context, "GridControlHeader"))
				using (templateService.Render(context, "GridControlHeaderRow"))
				{
					// check for columns with a filter
					if (columns.Any(candidate => candidate.HasFilter))
					{
						using (templateService.Render(context, "GridControlFilterRow"))
						{
							foreach (var column in columns)
								column.RenderHeaderFilter(context, templateService, dataset);
						}
					}

					// render all the columns
					foreach (var column in columns)
						column.RenderHeader(context, templateService, dataset);
				}

				// render the body of the gri
				using (templateService.Render(context, "GridControlBody"))
				{
					if (dataset.RowCount > 0)
					{
						// create the loop
						var loop = new Loop(dataset);

						// loop through all the rows
						using (context.Stack.Push("Loop", loop))
						{
							foreach (var row in loop.Rows)
							{
								// push the row to the stack
								using (context.Stack.Push("Row", row))
								using (templateService.Render(context, "GridControlBodyRow"))
								{
									foreach (var column in columns)
										column.RenderCell(context, templateService, dataset, row);
								}
							}
						}
					}
					else
					{
						// render the no result message
						templateService.Render(context, "GridControlRowNoResults").Dispose();
					}
				}
			}
		}
		#endregion
		#region Private Fields
		private readonly List<Column> columns = new List<Column>();
		#endregion
	}
}