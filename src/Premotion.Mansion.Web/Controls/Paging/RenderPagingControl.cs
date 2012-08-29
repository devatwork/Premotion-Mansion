using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Paging
{
	/// <summary>
	/// Renders the paging control.
	/// </summary>
	[ScriptFunction("RenderPagingControl")]
	public class RenderPagingControl : FunctionExpression
	{
		#region Constants
		/// <summary>
		/// Determines how many pages before/after the current page are displayed.
		/// </summary>
		private const int NumberOfAdjacentPagesToDisplay = 2;
		#endregion
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <param name="templateService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RenderPagingControl(IApplicationResourceService applicationResourceService, ITemplateService templateService)
		{
			// validate arguments
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");
			if (templateService == null)
				throw new ArgumentNullException("templateService");

			// set values
			this.applicationResourceService = applicationResourceService;
			this.templateService = templateService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Renders the paging control for the given dataset.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/>.</param>
		/// <param name="id">The ID of the dataset which to page.</param>
		/// <returns>Returns the HTML of the paging control.</returns>
		public string Evaluate(IMansionContext context, Dataset dataset, string id)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (dataset == null)
				throw new ArgumentNullException("dataset");
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");

			// check if this dataset does not support paging
			if (!dataset.IsPaged || dataset.RowCount == 0 || dataset.PageCount < 2)
				return string.Empty;

			// render the control
			var buffer = new StringBuilder();

			// get the control template
			var controlTemplateResourcePath = applicationResourceService.ParsePath(context, Control.ControlTemplatePathProperties);
			var controlTemplateResource = applicationResourceService.Get(context, controlTemplateResourcePath);

			// open the buffer pipe, control templates and render
			using (var pipe = new StringOutputPipe(buffer))
			using (context.OutputPipeStack.Push(pipe))
			using (templateService.Open(context, controlTemplateResource))
			{
				// construct the paging properties
				var pagingProperties = new PropertyBag
				                       {
				                       	{"id", id},
				                       	/* row values */
				                       	{"rowStart", ((dataset.CurrentPage - 1)*dataset.PageSize) + 1},
				                       	{"rowEnd", Math.Min((dataset.CurrentPage*dataset.PageSize), dataset.TotalSize)},
				                       	{"rowTotal", dataset.TotalSize},
				                       	/* page values */
				                       	{"currentPage", dataset.CurrentPage},
				                       	{"pageCount", dataset.PageCount},
				                       	{"pageSize", dataset.PageSize}
				                       };

				// render the control
				using (context.Stack.Push("ControlProperties", pagingProperties))
				using (context.Stack.Push("PagingProperties", pagingProperties))
				using (templateService.Render(context, "PagingControl", TemplateServiceConstants.OutputTargetField))
				{
					// render all the page options
					var displayPageNumberStart = Math.Max(dataset.CurrentPage - NumberOfAdjacentPagesToDisplay, 1);
					var displayPageNumberEnd = Math.Min(dataset.CurrentPage + NumberOfAdjacentPagesToDisplay, dataset.PageCount);
					for (var pageNumber = displayPageNumberStart; pageNumber <= displayPageNumberEnd; pageNumber++)
					{
						using (context.Stack.Push("PageOption", new PropertyBag
						                                        {
						                                        	{"number", pageNumber}
						                                        }))
							templateService.Render(context, "PagingControlPageOption").Dispose();
					}
				}
			}

			// return the buffer
			return buffer.ToString();
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly ITemplateService templateService;
		#endregion
	}
}