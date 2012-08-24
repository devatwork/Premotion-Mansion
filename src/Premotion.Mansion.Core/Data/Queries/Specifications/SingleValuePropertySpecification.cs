using System;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries.Specifications
{
	/// <summary>
	/// Represents a <see cref="PropertySpecification"/> with a single value.
	/// </summary>
	public abstract class SingleValuePropertySpecification : PropertySpecification
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="SingleValuePropertySpecification"/> for the given <paramref name="propertyName"/> and <paramref name="value"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to apply this specification.</param>
		/// <param name="value">The value on which to check.</param>
		/// <param name="op">The operator symbol.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		protected SingleValuePropertySpecification(string propertyName, object value, string op) : base(propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(op))
				throw new ArgumentNullException("op");

			// set values
			Value = value;
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
			builder.Append(PropertyName).Append(' ').Append(op).Append(' ').Append(Value);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the value on which to check in order to pass this specification.
		/// </summary>
		public object Value { get; private set; }
		#endregion
		#region Private Fields
		private readonly string op;
		#endregion
	}
}