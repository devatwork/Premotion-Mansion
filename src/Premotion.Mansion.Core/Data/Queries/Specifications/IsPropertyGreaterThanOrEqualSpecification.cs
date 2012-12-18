using System;

namespace Premotion.Mansion.Core.Data.Queries.Specifications
{
	/// <summary>
	/// Represents a &gt;= (is greater than or equal) specification on a property.
	/// </summary>
	public class IsPropertyGreaterThanOrEqualSpecification : SingleValuePropertySpecification
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="IsPropertyEqualSpecification"/> for the given <paramref name="propertyName"/> and <paramref name="value"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to apply this specification.</param>
		/// <param name="value">The value on which to check.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public IsPropertyGreaterThanOrEqualSpecification(string propertyName, object value) : base(propertyName, value, ">=")
		{
		}
		#endregion
	}
}