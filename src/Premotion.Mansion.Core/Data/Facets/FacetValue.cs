using System;

namespace Premotion.Mansion.Core.Data.Facets
{
	/// <summary>
	/// Represents a single facet value.
	/// </summary>
	public class FacetValue
	{
		#region Constructors
		/// <summary>
		/// Constructs this facet value.
		/// </summary>
		/// <param name="value">The value of this facet value.</param>
		/// <param name="count">The number of results for this facet value.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		public FacetValue(object value, int count)
		{
			// validate arguments
			if (value == null)
				throw new ArgumentNullException("value");

			// set values
			this.value = value;
			this.count = count;
			DisplayValue = value;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the value of this facet value.
		/// </summary>
		public object Value
		{
			get { return value; }
		}
		/// <summary>
		/// Gets the number of results for this facet value.
		/// </summary>
		public int Count
		{
			get { return count; }
		}
		/// <summary>
		/// Gets/Sets the display value of this facet value.
		/// </summary>
		public object DisplayValue { get; set; }
		#endregion
		#region Private Fields
		private readonly int count;
		private readonly object value;
		#endregion
	}
}