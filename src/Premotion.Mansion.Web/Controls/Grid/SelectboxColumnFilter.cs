using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Controls.Providers;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Implements a <see cref="ColumnFilter"/> using a Selectbox.
	/// </summary>
	public class SelectboxColumnFilter : ColumnFilter, IDataConsumerControl<DataProvider<Dataset>, Dataset>
	{
		#region Nested type: SelectboxColumnFilterFactoryTag
		/// <summary>
		/// Base class for <see cref="Premotion.Mansion.Web.Controls.Grid.SelectboxColumnFilter"/> factories.
		/// </summary>
		[ScriptTag(Constants.ControlTagNamespaceUri, "selectboxColumnFilter")]
		public class SelectboxColumnFilterFactoryTag : ColumnFilterFactoryTag
		{
			#region Overrides of ScriptTag
			/// <summary>
			/// Create a <see cref="ColumnFilter"/> instance.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created <see cref="ColumnFilter"/>.</returns>
			protected override ColumnFilter Create(IMansionWebContext context)
			{
				return new SelectboxColumnFilter();
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
			using (templateService.Render(context, "GridControl" + GetType().Name))
			{
				foreach (var row in Retrieve(context).Rows)
				{
					using (context.Stack.Push("OptionProperties", row))
						templateService.Render(context, "GridControl" + GetType().Name + "Option").Dispose();
				}
			}
		}
		#endregion
		#region Implementation of IDataConsumerControl<in DataProvider<Dataset>,Dataset>
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

			// set the provider
			provider = dataProvider;
		}
		/// <summary>
		/// Gets the data bound to this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the data.</returns>
		private Dataset Retrieve(IMansionContext context)
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