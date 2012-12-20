using System;
using System.Collections.Generic;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries.Specifications
{
	/// <summary>
	/// Negates all the <see cref="Specification"/>s within this specification.
	/// </summary>
	public class Negation : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs a negated <see cref="Specification"/> of the given <paramref name="specification"/>.
		/// </summary>
		/// <param name="specification">The <see cref="Specification"/> which to negate.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specification"/> is null.</exception>
		public Negation(Specification specification)
		{
			// validate arguments
			if (specification == null)
				throw new ArgumentNullException("specification");

			// set the value
			this.specification = specification;
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Gets the names of the properties used by this query component.
		/// </summary>
		/// <returns>Returns the property hints.</returns>
		protected override IEnumerable<string> DoGetPropertyHints()
		{
			return specification.GetPropertyHints();
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			// append the operator
			builder.Append("!");

			// append the string reprentation of the negated specification
			specification.AsString(builder);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the negated <see cref="Specification"/>.
		/// </summary>
		public Specification Negated
		{
			get { return specification; }
		}
		#endregion
		#region Private Fields
		private readonly Specification specification;
		#endregion
	}
}