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
			if (!dataset.IsPaged || dataset.RowCount == 0)
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
			using (templateService.Render(context, "StandAlonePagingControl", TemplateServiceConstants.OutputTargetField))
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
				using (context.Stack.Push("ControlProperties", pagingProperties, false))
				using (context.Stack.Push("PagingProperties", pagingProperties, false))
				using (templateService.Render(context, "PagingControl"))
				{
					// render all the page options
					for (var pageNumber = 1; pageNumber <= dataset.PageCount; pageNumber++)
					{
						using (context.Stack.Push("PageOption", new PropertyBag {{"number", pageNumber}}, false))
							templateService.Render(context, "PagingControlPageOption").Dispose();
					}

					// render the previous controls
					templateService.Render(context, "PreviousControls" + (dataset.CurrentPage == 1 ? "Disabled" : "Enabled")).Dispose();
					templateService.Render(context, "NextControls" + (dataset.CurrentPage == dataset.PageCount ? "Disabled" : "Enabled")).Dispose();
				}
			}

			// return the buffer
			return buffer.ToString();
		}
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly ITemplateService templateService;
		#endregion
	}
}