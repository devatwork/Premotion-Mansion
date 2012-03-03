using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Defines a sort on a single property.
	/// </summary>
	public class Sort
	{
		#region Constructor
		/// <summary>
		/// Constructs a sort for a property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="ascending">Flag indicating whether the sort is ascending (true) or descending (false).</param>
		private Sort(string propertyName, bool ascending)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			PropertyName = propertyName;
			Ascending = ascending;
		}
		#endregion
		#region Parse Methods
		/// <summary>
		/// Parses the <paramref name="sortString"/> into sorts.
		/// </summary>
		/// <param name="sortString">The sort string.</param>
		/// <returns>Returns the parsed sorts.</returns>
		public static IEnumerable<Sort> Parse(string sortString)
		{
			// validate arguments
			if (string.IsNullOrEmpty(sortString))
				return new Sort[0];

			// split the sort
			var sortStringParts = sortString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

			// convert the sortStrings into sorts
			return sortStringParts.Select(x => x.Trim()).Select(sortStringPart =>
			                                                    {
			                                                    	// check ascending
			                                                    	var ascending = sortStringPart.EndsWith("ASC", StringComparison.OrdinalIgnoreCase);
			                                                    	var propertyName = sortStringPart.Substring(0, sortStringPart.Length - (ascending ? 4 : 5));
			                                                    	return new Sort(propertyName, ascending);
			                                                    });
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property on which this sort is done.
		/// </summary>
		public string PropertyName { get; private set; }
		/// <summary>
		/// Gets a flag indicating whether the sort is ascending (true) or descending (false).
		/// </summary>
		public bool Ascending { get; private set; }
		#endregion
	}
}