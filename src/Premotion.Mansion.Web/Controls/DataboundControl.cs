using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Web.Controls.Providers;

namespace Premotion.Mansion.Web.Controls
{
	/// <summary>
	/// Base class for all databound controls.
	/// </summary>
	public abstract class DataboundControl<TDataType> : Control, IDataConsumerControl<DataProvider<TDataType>, TDataType>
	{
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected DataboundControl(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Data Methods
		/// <summary>
		/// Sets the <paramref name="dataProvider"/> for this control.
		/// </summary>
		/// <param name="dataProvider">The <see cref="DataProvider{TDataType}"/> which to set.</param>
		public void SetDataProvider(DataProvider<TDataType> dataProvider)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the data in form of <typeparamref name="TDataType"/>.</returns>
		protected TDataType Retrieve(IMansionContext context)
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
		private DataProvider<TDataType> provider;
		#endregion
	}
}