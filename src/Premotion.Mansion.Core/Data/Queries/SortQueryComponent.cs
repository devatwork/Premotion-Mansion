using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Represents a <see cref="Sort"/> <see cref="QueryComponent"/>.
	/// </summary>
	public class SortQueryComponent : QueryComponent
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="SortQueryComponent"/> from the given <paramref name="sorts"/>.
		/// </summary>
		/// <param name="sorts">The <see cref="Sort"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="sorts"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="sorts"/> is empty.</exception>
		public SortQueryComponent(IEnumerable<Sort> sorts)
		{
			// validate arguments
			if (sorts == null)
				throw new ArgumentNullException("sorts");

			// check against no sort
			var sortArray = sorts.ToArray();
			if (sortArray.Length == 0)
				throw new ArgumentException("Can not create a SortQueryComponent without a sort", "sorts");

			// set value
			this.sorts = sortArray;
		}
		#endregion
		#region Overrides of QueryComponent
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.AppendFormat("sort:{0}", string.Join("&", Sorts.Select(sort => sort.PropertyName + " " + (sort.Ascending ? "asc" : "desc"))));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="Sort"/>s.
		/// </summary>
		public IEnumerable<Sort> Sorts
		{
			get { return sorts; }
		}
		#endregion
		#region Private Fields
		private readonly IEnumerable<Sort> sorts;
		#endregion
	}
}