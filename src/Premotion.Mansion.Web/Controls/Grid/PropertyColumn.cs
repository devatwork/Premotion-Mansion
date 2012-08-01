using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents a column which is bound to a property.
	/// </summary>
	public class PropertyColumn : Column
	{
		#region Nested type: PropertyColumnFactoryTag
		/// <summary>
		/// Creates <see cref="PropertyColumn"/>s.
		/// </summary>
		[ScriptTag(Constants.ControlTagNamespaceUri, "propertyColumn")]
		public class PropertyColumnFactoryTag : ColumnFactoryTag
		{
			#region Overrides of ColumnFactoryTag
			/// <summary>
			/// Create a <see cref="Column"/> instance.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created <see cref="Column"/>.</returns>
			protected override Column Create(IMansionWebContext context)
			{
				return new PropertyColumn(GetAttributes(context))
				       {
				       	propertyName = GetRequiredAttribute<string>(context, "propertyName")
				       };
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="properties"></param>
		private PropertyColumn(IPropertyBag properties) : base(properties)
		{
		}
		#endregion
		#region Overrides of Column
		/// <summary>
		/// Renders a cell of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		/// <param name="row">The being rendered.</param>
		protected override void DoRenderCell(IMansionWebContext context, ITemplateService templateService, Dataset dataset, IPropertyBag row)
		{
			// create the cell properties
			var cellProperties = new PropertyBag
			                     {
			                     	{"value", row.Get<object>(context, propertyName, null)}
			                     };

			// render the cell
			using (context.Stack.Push("CellProperties", cellProperties))
				templateService.Render(context, "GridControlPropertyColumnContent").Dispose();
		}
		#endregion
		#region Private Fields
		private string propertyName;
		#endregion
	}
}