namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Implements the AND <see cref="CompositeFilter"/>.
	/// </summary>
	public class AndFilter : CompositeFilter
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public AndFilter() : base("and")
		{
		}
		#endregion
	}
}