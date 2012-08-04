using System;
using Premotion.Mansion.Core.Data.Specifications;

namespace Premotion.Mansion.Core.Data
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