using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Data.Specifications
{
	/// <summary>
	/// Represents an in list specification on a property.
	/// </summary>
	public class IsPropertyInSpecification : MultiValuePropertySpecification
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="IsPropertyInSpecification"/> for the given <paramref name="propertyName"/> and <paramref name="values"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to apply this specification.</param>
		/// <param name="values">The values on which to check.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> or <paramref name="values"/> is null.</exception>
		public IsPropertyInSpecification(string propertyName, IEnumerable<object> values) : base(propertyName, values, "in")
		{
		}
		#endregion
	}
}