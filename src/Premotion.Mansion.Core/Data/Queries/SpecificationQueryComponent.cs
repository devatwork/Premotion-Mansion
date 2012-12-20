using System;
using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Represents a <see cref="Specification"/> <see cref="QueryComponent"/>.
	/// </summary>
	public class SpecificationQueryComponent : QueryComponent
	{
		#region Constructors
		/// <summary>
		/// Construcs an <see cref="SpecificationQueryComponent"/> using the given <paramref name="specification"/>.
		/// </summary>
		/// <param name="specification">The <see cref="Specification"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specification"/> is null.</exception>
		public SpecificationQueryComponent(Specification specification)
		{
			// validate arguments
			if (specification == null)
				throw new ArgumentNullException("specification");

			// set value
			this.specification = specification;
		}
		#endregion
		#region Overrides of QueryComponent
		/// <summary>
		/// Gets the names of the properties used by this query component.
		/// </summary>
		/// <returns>Returns the property hints.</returns>
		protected override IEnumerable<string> DoGetPropertyHints()
		{
			return Specification.GetPropertyHints();
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			// append where
			builder.Append("where:");

			// append specification
			Specification.AsString(builder);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Get the <see cref="Specification"/>.
		/// </summary>
		public Specification Specification
		{
			get { return specification; }
		}
		#endregion
		#region Private Fields
		private readonly Specification specification;
		#endregion
	}
}