using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Converts a <see cref="Column"/> into a sortable column.
	/// </summary>
	public class PropertyColumnSort : ColumnSort
	{
		#region Nested type: PropertyColumnSorterFactoryTag
		/// <summary>
		/// Constructs <see cref="PropertyColumnSort"/>s.
		/// </summary>
		[Named(Constants.ControlTagNamespaceUri, "propertyColumnSort")]
		public class PropertyColumnSorterFactoryTag : ColumnSorterFactoryTag
		{
			#region Overrides of ColumnSorterFactoryTag
			/// <summary>
			/// Creates a <see cref="ColumnSort"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="column">The <see cref="Column"/> to which the sort is applied.</param>
			/// <param name="properties">The properties of the filter.</param>
			/// <returns>Returns the created <see cref="ColumnSort"/>.</returns>
			protected override ColumnSort Create(MansionContext context, Column column, IPropertyBag properties)
			{
				return new PropertyColumnSort(GetRequiredAttribute<string>(context, "property"), properties);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column sort.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to sort.</param>
		/// <param name="properties">The properties of this sort.</param>
		private PropertyColumnSort(string propertyName, IPropertyBag properties) : base(properties)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			this.propertyName = propertyName;
		}
		#endregion
		#region Overrides of ColumnSort
		/// <summary>
		/// Renders this column sort.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/>.</param>
		protected override void DoRender(MansionWebContext context, ITemplateService templateService, Dataset data)
		{
			// create the header properties
			var headerProperties = new PropertyBag();

			// get the current sort of the grid
			var currentSort = data.Sorts.FirstOrDefault();
			if (currentSort != null && currentSort.PropertyName.StartsWith(propertyName, StringComparison.OrdinalIgnoreCase))
			{
				headerProperties.Set("direction", currentSort.Ascending ? "asc" : "desc");
				headerProperties.Set("sortParameter", propertyName + " " + (currentSort.Ascending ? "desc" : "asc"));
			}
			else
				headerProperties.Set("sortParameter", propertyName + " asc");

			// push the column properties to the stack
			using (context.Stack.Push("HeaderProperties", headerProperties, false))
				templateService.Render(context, "GridControlSortableHeaderCell").Dispose();
		}
		#endregion
		#region Private Fields
		private readonly string propertyName;
		#endregion
	}
}