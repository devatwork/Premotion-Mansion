namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Represents a limit <see cref="QueryComponent"/>.
	/// </summary>
	public class LimitQueryComponent : QueryComponent
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="SortQueryComponent"/> from the given <paramref name="limit"/>.
		/// </summary>
		/// <param name="limit">The number of items on which to limit.</param>
		public LimitQueryComponent(int limit)
		{
			// set value
			this.limit = limit;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets maximum number of results returned from this query.
		/// </summary>
		public int Limit
		{
			get { return limit; }
		}
		#endregion
		#region Private Fields
		private readonly int limit;
		#endregion
	}
}