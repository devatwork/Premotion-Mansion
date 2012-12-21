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
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Empty;
		}
		#endregion
	}
}