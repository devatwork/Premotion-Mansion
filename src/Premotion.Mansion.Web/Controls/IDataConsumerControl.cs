using Premotion.Mansion.Web.Controls.Providers;

namespace Premotion.Mansion.Web.Controls
{
	/// <summary>
	/// Interface for all consumers of <see cref="DataProvider{TDataType}"/>.
	/// </summary>
	public interface IDataConsumerControl<in TProviderType, TDataType> : IControl where TProviderType : DataProvider<TDataType>
	{
		#region Set Methods
		/// <summary>
		/// Sets the <paramref name="dataProvider"/> for this control.
		/// </summary>
		/// <param name="dataProvider">The <see cref="DataProvider{TDataType}"/> which to set.</param>
		void SetDataProvider(TProviderType dataProvider);
		#endregion
	}
}