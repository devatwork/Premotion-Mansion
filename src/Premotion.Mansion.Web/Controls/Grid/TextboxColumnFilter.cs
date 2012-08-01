using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Implements a <see cref="ColumnFilter"/> using a textbox.
	/// </summary>
	public class TextboxColumnFilter : ColumnFilter
	{
		#region Nested type: TextboxColumnFilterFactoryTag
		/// <summary>
		/// Base class for <see cref="TextboxColumnFilter"/> factories.
		/// </summary>
		[ScriptTag(Constants.ControlTagNamespaceUri, "textboxColumnFilter")]
		public class TextboxColumnFilterFactoryTag : ColumnFilterFactoryTag
		{
			#region Overrides of ScriptTag
			/// <summary>
			/// Create a <see cref="ColumnFilter"/> instance.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created <see cref="ColumnFilter"/>.</returns>
			protected override ColumnFilter Create(IMansionWebContext context)
			{
				return new TextboxColumnFilter();
			}
			#endregion
		}
		#endregion
		#region Overrides of ColumnFilter
		/// <summary>
		/// Renders the header of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		protected override void DoRenderHeader(IMansionWebContext context, ITemplateService templateService, Dataset dataset)
		{
			templateService.Render(context, "GridControl" + GetType().Name).Dispose();
		}
		#endregion
	}
}