using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Represents a limit <see cref="QueryComponent"/>.
	/// </summary>
	public class CacheQueryComponent : QueryComponent
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="CacheQueryComponent"/> from the given <paramref name="isEnabled"/>.
		/// </summary>
		/// <param name="isEnabled"></param>
		public CacheQueryComponent(bool isEnabled)
		{
			// set value
			this.isEnabled = isEnabled;
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
			builder.AppendFormat("cache-enabled:{0}", IsEnabled);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether the result of this query can be cached or not.
		/// </summary>
		public bool IsEnabled
		{
			get { return isEnabled; }
		}
		#endregion
		#region Private Fields
		private readonly bool isEnabled;
		#endregion
	}
}