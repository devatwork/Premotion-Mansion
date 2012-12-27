using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Represents a storage-only <see cref="QueryComponent"/>.
	/// </summary>
	public class StorageOnlyQueryComponent : QueryComponent
	{
		#region Constants
		/// <summary>
		/// Storage only key name.
		/// </summary>
		public const string PropertyKey = @"_storageOnly";
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
			builder.Append("storage-only:true");
		}
		#endregion
	}
}