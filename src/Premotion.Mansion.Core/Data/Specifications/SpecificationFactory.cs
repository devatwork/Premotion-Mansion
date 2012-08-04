using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Data.Specifications
{
	/// <summary>
	/// Provides factory methods for <see cref="Specification"/>s.
	/// </summary>
	public static class SpecificationFactory
	{
		#region Composite Specfication Methods
		/// <summary>
		/// Constructs an specification which combines the given <paramref name="specifications"/> using the logical AND operator.
		/// </summary>
		/// <param name="specifications">The <see cref="Specification"/>s which to combine.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specifications"/> is null.</exception>
		public static Specification And(IEnumerable<Specification> specifications)
		{
			// validate arguments
			if (specifications == null)
				throw new ArgumentNullException("specifications");

			// create the specification
			return new Conjunction().Add(specifications);
		}
		/// <summary>
		/// Constructs an specification which combines the given <paramref name="specifications"/> using the logical OR operator.
		/// </summary>
		/// <param name="specifications">The <see cref="Specification"/>s which to combine.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specifications"/> is null.</exception>
		public static Specification Or(IEnumerable<Specification> specifications)
		{
			// validate arguments
			if (specifications == null)
				throw new ArgumentNullException("specifications");

			// create the specification
			return new Disjunction().Add(specifications);
		}
		/// <summary>
		/// Constructs a negated <see cref="Specification"/> of the given <paramref name="specification"/>.
		/// </summary>
		/// <param name="specification">The <see cref="Specification"/> which to negate.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specification"/> is null.</exception>
		public static Specification Not(Specification specification)
		{
			// validate arguments
			if (specification == null)
				throw new ArgumentNullException("specification");

			// create the specification
			return new Negation(specification);
		}
		#endregion
		#region Property Specification Methods
		/// <summary>
		/// Constructs an == (is equal) specfication.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to apply this specification.</param>
		/// <param name="value">The value on which to check.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public static Specification IsEqual(string propertyName, object value)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// create the specfification
			return new IsPropertyEqualSpecification(propertyName, value);
		}
		/// <summary>
		/// Constructs an in list specification.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to apply this specification.</param>
		/// <param name="values">The values on which to check.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> or <paramref name="values"/> is null.</exception>
		public static Specification IsIn(string propertyName, params object[] values)
		{
			return IsIn(propertyName, (IEnumerable<object>) values);
		}
		/// <summary>
		/// Constructs an in list specification.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to apply this specification.</param>
		/// <param name="values">The values on which to check.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> or <paramref name="values"/> is null.</exception>
		public static Specification IsIn(string propertyName, IEnumerable<object> values)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (values == null)
				throw new ArgumentNullException("values");

			// create the specification
			return new IsPropertyInSpecification(propertyName, values);
		}
		#endregion
	}
}