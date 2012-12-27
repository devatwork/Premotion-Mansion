using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries
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
		#region Overrides of QueryComponent
		/// <summary>
		/// Gets the names of the properties used by this query component.
		/// </summary>
		/// <returns>Returns the property hints.</returns>
		protected override IEnumerable<string> DoGetPropertyHints()
		{
			return Enumerable.Empty<string>();
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.AppendFormat("limit:{0}", Limit);
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