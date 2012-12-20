using System;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Sorts
{
	/// <summary>
	/// Implements the field sort.
	/// </summary>
	public class FieldSort : BaseSort
	{
		#region Constructors
		/// <summary>
		/// Constructs a field sort.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to sort.</param>
		/// <param name="ascending">Flag indacating whether the sort is ascending or descending.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public FieldSort(string propertyName, bool ascending)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			this.propertyName = propertyName;
			this.ascending = ascending;
		}
		#endregion
		#region Private Fields
		private readonly bool ascending;
		private readonly string propertyName;
		#endregion
	}
}