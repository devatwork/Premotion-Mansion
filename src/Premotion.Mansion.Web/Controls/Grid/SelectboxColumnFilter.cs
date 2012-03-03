using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Controls.Providers;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Implements <see cref="ColumnFilter"/> using a selectbox.
	/// </summary>
	public class SelectboxColumnFilter : ColumnFilter, IDataConsumerControl<DataProvider<Dataset>, Dataset>
	{
		#region Nested type: SelectboxColumnFilterFactoryTag
		/// <summary>
		/// Constructs <see cref="TextboxColumnFilter"/>s.
		/// </summary>
		[Named(Constants.ControlTagNamespaceUri, "selectboxColumnFilter")]
		public class SelectboxColumnFilterFactoryTag : ColumnFilterFactoryTag
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
				return new SelectboxColumnFilter(properties);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column filter.
		/// </summary>
		/// <param name="properties">The properties of this filter.</param>
		private SelectboxColumnFilter(IPropertyBag properties) : base(properties)
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
			using (templateService.Render(context, "SelectboxColumnFilter"))
			{
				foreach (var row in Retrieve(context).Rows)
				{
					using (context.Stack.Push("OptionProperties", row, false))
						templateService.Render(context, "SelectboxColumnFilterOption").Dispose();
				}
			}
		}
		#endregion
		#region Data Methods
		/// <summary>
		/// Sets the <paramref name="dataProvider"/> for this control.
		/// </summary>
		/// <param name="dataProvider">The <see cref="DataProvider{TDataType}"/> which to set.</param>
		public void SetDataProvider(DataProvider<Dataset> dataProvider)
		{
			// validate arguments
			if (dataProvider == null)
				throw new ArgumentNullException("dataProvider");
			if (provider != null)
				throw new InvalidOperationException("The data provider has already been set, cant override it.");

			// set value
			provider = dataProvider;
		}
		/// <summary>
		/// Gets the data bound to this control.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the data.</returns>
		private Dataset Retrieve(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if no provider was set
			if (provider == null)
				throw new InvalidOperationException("No data provider was set.");

			// return the data from the provider
			return provider.Retrieve(context);
		}
		#endregion
		#region Private Fields
		private DataProvider<Dataset> provider;
		#endregion
	}
}