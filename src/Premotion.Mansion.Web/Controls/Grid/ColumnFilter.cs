using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents the filter options of a column.
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
			protected override sealed void DoExecute(IMansionContext context)
			{
				// get the web context
				var webContext = context.Cast<IMansionWebContext>();

				// get the column
				Column column;
				if (!webContext.TryPeekControl(out column))
					throw new InvalidOperationException(string.Format("'{0}' must be added to a '{1}'", GetType(), typeof (Column)));

				// create the filter
				var filter = Create(webContext);
				filter.Properties = GetAttributes(context);

				// allow facets
				using (webContext.ControlStack.Push(filter))
					ExecuteChildTags(webContext);

				// set the sort to the column
				column.Filter = filter;
			}
			/// <summary>
			/// Create a <see cref="ColumnFilter"/> instance.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created <see cref="ColumnFilter"/>.</returns>
			protected abstract ColumnFilter Create(IMansionWebContext context);
			#endregion
		}
		#endregion
		#region Render Methods
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

			// invoke the template
			using (context.Stack.Push("FilterProperties", Properties))
				DoRenderHeader(context, templateService, dataset);
		}
		/// <summary>
		/// Renders the header of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		protected abstract void DoRenderHeader(IMansionWebContext context, ITemplateService templateService, Dataset dataset);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="IPropertyBag"/> of this filter.
		/// </summary>
		protected IPropertyBag Properties { get; private set; }
		#endregion
	}
}