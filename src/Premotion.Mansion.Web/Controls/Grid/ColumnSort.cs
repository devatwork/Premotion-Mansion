using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents the sort of an <see cref="Column"/>.
	/// </summary>
	public abstract class ColumnSort
	{
		#region Nested type: ColumnSorterFactoryTag
		/// <summary>
		/// Base class for <see cref="ColumnSort"/> factories.
		/// </summary>
		public abstract class ColumnSorterFactoryTag : ScriptTag
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
				var sort = Create(context, column, properties);

				// set the sort to the column
				column.Set(sort);
			}
			/// <summary>
			/// Creates a <see cref="ColumnSort"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="column">The <see cref="Column"/> to which the sort is applied.</param>
			/// <param name="properties">The properties of the filter.</param>
			/// <returns>Returns the created <see cref="ColumnSort"/>.</returns>
			protected abstract ColumnSort Create(IMansionContext context, Column column, IPropertyBag properties);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column sort.
		/// </summary>
		/// <param name="properties">The properties of this sort.</param>
		protected ColumnSort(IPropertyBag properties)
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

			using (context.Stack.Push("ColumnSortProperties", Properties, false))
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
		/// Gets the properties of this sort.
		/// </summary>
		protected IPropertyBag Properties { get; private set; }
		#endregion
	}
}