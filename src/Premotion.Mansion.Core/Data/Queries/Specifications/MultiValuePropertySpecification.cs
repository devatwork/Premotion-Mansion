using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries.Specifications
{
	/// <summary>
	/// Represents a <see cref="PropertySpecification"/> with multiple values.
	/// </summary>
	public abstract class MultiValuePropertySpecification : PropertySpecification
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="MultiValuePropertySpecification"/> for the given <paramref name="propertyName"/> and <paramref name="values"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to apply this specification.</param>
		/// <param name="values">The values on which to check.</param>
		/// <param name="op">The operator symbol.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> or <paramref name="values"/> is null.</exception>
		protected MultiValuePropertySpecification(string propertyName, IEnumerable<object> values, string op) : base(propertyName)
		{
			// validate arguments
			if (values == null)
				throw new ArgumentNullException("values");
			if (string.IsNullOrEmpty(op))
				throw new ArgumentNullException("op");

			// set values
			this.values = values.ToArray();
			this.op = op;
		}
		#endregion
		#region Overrides of SpecificationBase
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append(PropertyName).Append(' ').Append(op).Append(' ').Append(Values);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the values on which to check in order to pass this specification.
		/// </summary>
		public IEnumerable<object> Values
		{
			get { return values; }
		}
		#endregion
		#region Private Fields
		private readonly string op;
		private readonly object[] values;
		#endregion
	}
}