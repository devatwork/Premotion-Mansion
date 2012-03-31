using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Providers
{
	/// <summary>
	/// Base class for all data provider.
	/// </summary>
	/// <typeparam name="TDataType">The type of data which is retrieved by this provider.</typeparam>
	public abstract class DataProvider<TDataType>
	{
		#region Nested type: DataProviderFactoryTag
		/// <summary>
		/// Base class for <see cref="DataProvider{TDataType}"/> factories.
		/// </summary>
		public abstract class DataProviderFactoryTag<TProviderType> : ScriptTag where TProviderType : DataProvider<TDataType>
		{
			#region Overrides of ScriptTag
			/// <summary>
			/// Executes this tag.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			protected override void DoExecute(IMansionContext context)
			{
				// get the web context
				var webContext = context.Cast<IMansionWebContext>();

				// create the provider
				var provider = Create(webContext);
				if (provider == null)
					throw new InvalidOperationException("DataProviders must always be non-null");

				// get the top-most data provider consumer control
				InjectProviderInConsumer(webContext, provider);
			}
			/// <summary>
			/// Ibjects the <paramref name="provider"/> into the proper consumer.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="provider">The <typeparamref name="TProviderType"/>.</param>
			protected virtual void InjectProviderInConsumer(IMansionWebContext context, TProviderType provider)
			{
				// get the top-most data provider consumer control
				IDataConsumerControl<TProviderType, TDataType> consumer;
				if (!context.TryFindControl(out consumer))
					throw new InvalidOperationException(string.Format("No data consumer for provider '{0}' was found on the control stack", GetType()));

				// set the provider
				consumer.SetDataProvider(provider);
			}
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected abstract TProviderType Create(IMansionWebContext context);
			#endregion
		}
		#endregion
		#region Data Methods
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <typeparamref name="TDataType"/>.</returns>
		public TDataType Retrieve(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			return DoRetrieve(context);
		}
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected abstract TDataType DoRetrieve(IMansionContext context);
		#endregion
	}
}