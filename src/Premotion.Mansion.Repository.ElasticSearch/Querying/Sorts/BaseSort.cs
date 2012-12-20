namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Sorts
{
	/// <summary>
	/// Base class for all sorts.
	/// </summary>
	public abstract class BaseSort
	{
		#region Nested type: BaseSortConverter
		/// <summary>
		/// Converts <see cref="BaseSort"/>s.
		/// </summary>
		protected abstract class BaseSortConverter<TSort> : BaseConverter<TSort> where TSort : BaseSort
		{
		}
		#endregion
	}
}