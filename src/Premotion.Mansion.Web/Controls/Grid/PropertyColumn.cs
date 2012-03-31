using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents a grid column displaying a property.
	/// </summary>
	public class PropertyColumn : Column
	{
		#region Nested type: PropertyColumnFactoryTag
		/// <summary>
		/// Constructs <see cref="PropertyColumn"/>s/
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
				// get the property name
				var propertyName = GetRequiredAttribute<string>(context, "property");

				// create the column
				return new PropertyColumn(GetAttributes(context), propertyName);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="properties">The properties of this column.</param>
		/// <param name="propertyName">The name of the property displayed by this column.</param>
		private PropertyColumn(IPropertyBag properties, string propertyName) : base(properties)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			PropertyName = propertyName;
		}
		#endregion
		#region Overrides of Column
		/// <summary>
		/// Renders a cell of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/> rendered in this column.</param>
		/// <param name="row">The being rendered.</param>
		protected override void DoRenderCell(IMansionWebContext context, ITemplateService templateService, Dataset data, IPropertyBag row)
		{
			// create the cell properties
			var cellProperties = new PropertyBag
			                     {
			                     	{"value", row.Get<object>(context, PropertyName, null)}
			                     };

			// render the cell
			using (context.Stack.Push("CellProperties", cellProperties, false))
				templateService.Render(context, "GridControlPropertyColumnContent").Dispose();
		}
		#endregion
		#region Private Fields
		/// <summary>
		/// Gets the name of the property displayed by this column.
		/// </summary>
		public string PropertyName { get; private set; }
		#endregion
	}
}