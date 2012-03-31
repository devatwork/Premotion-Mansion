using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Implements a <see cref="ColumnFilter"/> without a filter.
	/// </summary>
	public class NoColumnFilter : ColumnFilter
	{
		#region Constructors
		/// <summary>
		/// Constructs a column filter.
		/// </summary>
		/// <param name="properties">The properties of this filter.</param>
		private NoColumnFilter(IPropertyBag properties) : base(properties)
		{
		}
		#endregion
		#region Overrides of ColumnFilter
		/// <summary>
		/// Renders this column sort.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService, Dataset data)
		{
			templateService.Render(context, "NoFilterColumnFilter").Dispose();
		}
		#endregion
		#region Singleton Properties
		/// <summary>
		/// Gets the instance.
		/// </summary>
		public static NoColumnFilter Instance
		{
			get { return singleton.Value; }
		}
		#endregion
		#region Private Fields
		private static readonly Lazy<NoColumnFilter> singleton = new Lazy<NoColumnFilter>(() => new NoColumnFilter(new PropertyBag()));
		#endregion
	}
}