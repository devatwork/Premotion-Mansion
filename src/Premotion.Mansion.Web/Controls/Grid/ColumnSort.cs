using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents the sorting options of a column.
	/// </summary>
	public class ColumnSort : IControl
	{
		#region Nested type: ColumnSortFactoryTag
		/// <summary>
		/// Base class for <see cref="ColumnSort"/> factories.
		/// </summary>
		[ScriptTag(Constants.ControlTagNamespaceUri, "columnSort")]
		public class ColumnSortFactoryTag : ScriptTag
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

				// get the property names on which to sort
				var propertyName = GetRequiredAttribute<string>(context, "on");

				// create the filter
				var sort = new ColumnSort
				           {
				           	PropertyName = propertyName
				           };

				// allow facets
				using (webContext.ControlStack.Push(sort))
					ExecuteChildTags(webContext);

				// set the sort to the column
				column.Sort = sort;
			}
			#endregion
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Renders the header of the column on which this sorter is working.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void RenderHeader(IMansionWebContext context, ITemplateService templateService, Dataset dataset)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (dataset == null)
				throw new ArgumentNullException("dataset");

			// determine if this column sort is active
			var activeSort = dataset.Sorts.FirstOrDefault();

			// check if there is an active sort
			var active = false;
			var ascending = false;
			if (activeSort != null && PropertyName.Equals(activeSort.PropertyName, StringComparison.OrdinalIgnoreCase))
			{
				active = true;
				ascending = activeSort.Ascending;
			}

			// create the sort properties
			var properties = new PropertyBag
			                 {
			                 	{"active", active},
			                 	{"direction", ascending},
			                 	{"sortParameter", PropertyName + " " + (ascending ? "desc" : "asc")}
			                 };

			using (context.Stack.Push("ColumnSortProperties", properties))
				templateService.Render(context, "GridControl" + GetType().Name + "Header").Dispose();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property on which to sort.
		/// </summary>
		private string PropertyName { get; set; }
		#endregion
	}
}