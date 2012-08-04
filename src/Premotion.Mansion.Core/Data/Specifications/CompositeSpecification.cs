using System;
using System.Collections.Generic;
using System.Text;

namespace Premotion.Mansion.Core.Data.Specifications
{
	/// <summary>
	/// Represents a collection of related <see cref="Specification"/>s.
	/// </summary>
	public class CompositeSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="CompositeSpecification"/>.
		/// </summary>
		/// <param name="op">The operator.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="op"/> is null or empty.</exception>
		protected CompositeSpecification(string op)
		{
			// validate arguments
			if (string.IsNullOrEmpty(op))
				throw new ArgumentNullException("op");

			// set the value
			this.op = op;
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append('(');
			var first = true;
			foreach (var component in Components)
			{
				// append the string representation of the component
				component.AsString(builder);

				// check for first component
				if (first)
				{
					first = false;
					continue;
				}

				// append separator
				builder.Append(' ');
				builder.Append(op);
				builder.Append(' ');
			}
			builder.Append(')');
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds the given <paramref name="specification"/> to this composite specification.
		/// </summary>
		/// <param name="specification">The <see cref="Specification"/> which to add.</param>
		/// <returns>Returns the instance to allow chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specification"/> is null.</exception>
		public CompositeSpecification Add(Specification specification)
		{
			// validate arguments
			if (specification == null)
				throw new ArgumentNullException("specification");

			// add to the list
			components.Add(specification);

			// return this to allow chaining
			return this;
		}
		/// <summary>
		/// Adds the given <paramref name="specifications"/> to this composite specification.
		/// </summary>
		/// <param name="specifications">The <see cref="Specification"/>s which to add.</param>
		/// <returns>Returns the instance to allow chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specifications"/> is null.</exception>
		public CompositeSpecification Add(IEnumerable<Specification> specifications)
		{
			// validate arguments
			if (specifications == null)
				throw new ArgumentNullException("specifications");

			// add to the list
			components.AddRange(specifications);

			// return this to allow chaining
			return this;
		}
		/// <summary>
		/// Adds the given <paramref name="specifications"/> to this composite specification.
		/// </summary>
		/// <param name="specifications">The <see cref="Specification"/>s which to add.</param>
		/// <returns>Returns the instance to allow chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specifications"/> is null.</exception>
		public CompositeSpecification Add(params Specification[] specifications)
		{
			return Add((IEnumerable<Specification>) specifications);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="Specification"/>s which make up this composition.
		/// </summary>
		public IEnumerable<Specification> Components
		{
			get { return components; }
		}
		#endregion
		#region Private Fields
		private readonly List<Specification> components = new List<Specification>();
		private readonly string op;
		#endregion
	}
}