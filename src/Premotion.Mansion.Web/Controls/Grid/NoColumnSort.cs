using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Implements a <see cref="ColumnSort"/> without a sort.
	/// </summary>
	public class NoColumnSort : ColumnSort
	{
		#region Constructors
		/// <summary>
		/// Constructs a column sort.
		/// </summary>
		/// <param name="properties">The properties of this sort.</param>
		private NoColumnSort(IPropertyBag properties) : base(properties)
		{
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
			templateService.Render(context, "GridControlHeaderCell").Dispose();
		}
		#endregion
		#region Singleton Properties
		/// <summary>
		/// Gets the instance.
		/// </summary>
		public static NoColumnSort Instance
		{
			get { return singleton.Value; }
		}
		#endregion
		#region Private Fields
		private static readonly Lazy<NoColumnSort> singleton = new Lazy<NoColumnSort>(() => new NoColumnSort(new PropertyBag()));
		#endregion
	}
}