using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Implements <see cref="ColumnFilter"/> using a textbox.
	/// </summary>
	public class TextboxColumnFilter : ColumnFilter
	{
		#region Nested type: TextboxColumnFilterFactoryTag
		/// <summary>
		/// Constructs <see cref="TextboxColumnFilter"/>s.
		/// </summary>
		[Named(Constants.ControlTagNamespaceUri, "textboxColumnFilter")]
		public class TextboxColumnFilterFactoryTag : ColumnFilterFactoryTag
		{
			#region Overrides of ColumnFilterFactoryTag
			/// <summary>
			/// Creates a <see cref="ColumnFilter"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="column">The <see cref="Column"/> to which this filter is applied.</param>
			/// <param name="properties">The properties of the filter.</param>
			/// <returns>Returns the created <see cref="ColumnFilter"/>.</returns>
			protected override ColumnFilter Create(MansionContext context, Column column, IPropertyBag properties)
			{
				return new TextboxColumnFilter(properties);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column filter.
		/// </summary>
		/// <param name="properties">The properties of this filter.</param>
		private TextboxColumnFilter(IPropertyBag properties) : base(properties)
		{
		}
		#endregion
		#region Overrides of ColumnFilter
		/// <summary>
		/// Renders this column sort.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/>.</param>
		protected override void DoRender(MansionWebContext context, ITemplateService templateService, Dataset data)
		{
			templateService.Render(context, "TextboxColumnFilter").Dispose();
		}
		#endregion
	}
}