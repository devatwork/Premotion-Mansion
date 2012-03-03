using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Web.Controls.Providers.Datasets
{
	/// <summary>
	/// Provides data by fetching a <see cref="Dataset"/> from the <see cref="MansionContext.Stack"/>.
	/// </summary>
	public class FetchDatasetProvider : DatasetProvider
	{
		#region Nested type: FetchDatasetProviderFactoryTag
		/// <summary>
		/// Creates <see cref="FetchDatasetProvider"/>s.
		/// </summary>
		[Named(Constants.DataProviderTagNamespaceUri, "fetchDatasetProvider")]
		public class FetchDatasetProviderFactoryTag : DatasetProviderFactoryTag<DatasetProvider>
		{
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override DatasetProvider Create(MansionWebContext context)
			{
				return new FetchDatasetProvider(GetRequiredAttribute<Dataset>(context, "source"));
			}
			#endregion
		}
		#endregion
		#region Nested type: ScriptedDatasetProviderFactoryTag
		/// <summary>
		/// Creates <see cref="FetchDatasetProvider"/>s.
		/// </summary>
		[Named(Constants.DataProviderTagNamespaceUri, "scriptedDatasetProvider")]
		public class ScriptedDatasetProviderFactoryTag : DatasetProviderFactoryTag<DatasetProvider>
		{
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override DatasetProvider Create(MansionWebContext context)
			{
				// create the dataset
				var dataset = new Dataset();
				using (context.Stack.Push("Dataset", dataset, false))
					ExecuteChildTags(context);

				return new FetchDatasetProvider(dataset);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a fetch dataset provider.
		/// </summary>
		/// <param name="dataset">The <see cref="Dataset"/> which to provide.</param>
		private FetchDatasetProvider(Dataset dataset)
		{
			// validate arguments
			if (dataset == null)
				throw new ArgumentNullException("dataset");

			// set values
			this.dataset = dataset;
		}
		#endregion
		#region Overrides of DataProvider<Dataset>
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected override Dataset DoRetrieve(MansionContext context)
		{
			return dataset;
		}
		#endregion
		#region Private Fields
		private readonly Dataset dataset;
		#endregion
	}
}