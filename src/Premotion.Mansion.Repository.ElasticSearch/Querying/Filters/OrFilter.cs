namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Implements the OR <see cref="CompositeFilter"/>.
	/// </summary>
	public class OrFilter : CompositeFilter
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public OrFilter() : base("or")
		{
		}
		#endregion
	}
}