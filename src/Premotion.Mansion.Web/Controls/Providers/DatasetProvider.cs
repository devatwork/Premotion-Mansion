using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Web.Controls.Providers
{
	/// <summary>
	/// Base class for all <see cref="DatasetProvider{TDatasetType}"/>s returning <see cref="Dataset"/>.
	/// </summary>
	public abstract class DatasetProvider : DatasetProvider<Dataset>
	{
	}
	/// <summary>
	/// Base class for all <see cref="DataProvider{TDataType}"/>s returning <see cref="Dataset"/>.
	/// </summary>
	/// <typeparam name="TDatasetType">The type of <see cref="Dataset"/> returned by this provider.</typeparam>
	public abstract class DatasetProvider<TDatasetType> : DataProvider<TDatasetType> where TDatasetType : Dataset
	{
		#region Nested type: DatasetProviderFactoryTag
		/// <summary>
		/// Factory class for all <see cref="DatasetProvider{TDatasetType}"/>.
		/// </summary>
		/// <typeparam name="TProviderType">The type of <see cref="DatasetProvider{TDatasetType}"/>.</typeparam>
		public abstract class DatasetProviderFactoryTag<TProviderType> : DataProviderFactoryTag<TProviderType> where TProviderType : DatasetProvider<TDatasetType>
		{
			#region Nested type: DatasetProviderAdapter
			/// <summary>
			/// Adapts TProviderType to <see cref="DatasetProvider"/>.
			/// </summary>
			private class DatasetProviderAdapter : DatasetProvider
			{
				#region Constructor
				/// <summary>
				/// Constructs this adapter.
				/// </summary>
				/// <param name="provider">The <see cref="DatasetProvider{TDatasetType}"/> which to adapt.</param>
				public DatasetProviderAdapter(TProviderType provider)
				{
					this.provider = provider;
				}
				#endregion
				#region Overrides of DataProvider<Dataset>
				/// <summary>
				/// Retrieves the data from this provider.
				/// </summary>
				/// <param name="context">The <see cref="IMansionContext"/>.</param>
				/// <returns>Returns the retrieve data.</returns>
				protected override Dataset DoRetrieve(IMansionContext context)
				{
					return provider.DoRetrieve(context);
				}
				#endregion
				#region Private Fields
				private readonly TProviderType provider;
				#endregion
			}
			#endregion
			#region Overrides of DataProviderFactoryTag{TProviderType}
			/// <summary>
			/// Ibjects the <paramref name="provider"/> into the proper consumer.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="provider">The <typeparamref name="TProviderType"/>.</param>
			protected override void InjectProviderInConsumer(IMansionWebContext context, TProviderType provider)
			{
				// try to inject using the native format
				IDataConsumerControl<TProviderType, TDatasetType> nativeConsumer;
				if (context.TryFindControl(out nativeConsumer))
				{
					nativeConsumer.SetDataProvider(provider);
					return;
				}

				// try to inject using the dataset format
				IDataConsumerControl<DatasetProvider<Dataset>, Dataset> datasetConsumer;
				if (context.TryFindControl(out datasetConsumer))
				{
					datasetConsumer.SetDataProvider(new DatasetProviderAdapter(provider));
					return;
				}

				throw new InvalidOperationException(string.Format("No data consumer for provider '{0}' was found on the control stack", GetType()));
			}
			#endregion
		}
		#endregion
	}
}