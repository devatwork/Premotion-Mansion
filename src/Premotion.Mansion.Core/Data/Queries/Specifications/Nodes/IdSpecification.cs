using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries.Specifications.Nodes
{
	/// <summary>
	/// Specifies IDs.
	/// </summary>
	public class IdSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs this query part with the specified base type.
		/// </summary>
		/// <param name="ids">The types.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="ids"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="ids"/> is empty.</exception>
		private IdSpecification(IEnumerable<int> ids)
		{
			// validate arguments
			if (ids == null)
				throw new ArgumentNullException("ids");

			// guard against empty types
			var idArray = ids.ToArray();
			if (idArray.Length == 0)
				throw new InvalidOperationException("The IdSpecification should at least include one ID");

			// set value
			IDs = idArray;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs a <see cref="IdSpecification"/> specification matching the given <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The ID.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="id"/> is empty.</exception>
		public static IdSpecification Is(int id)
		{
			return IsAny(id);
		}
		/// <summary>
		/// Constructs a <see cref="IdSpecification"/> specification matching any of the given <paramref name="ids"/>.
		/// </summary>
		/// <param name="ids">The IDs.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="ids"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="ids"/> is empty.</exception>
		public static IdSpecification IsAny(params int[] ids)
		{
			return new IdSpecification(ids);
		}
		/// <summary>
		/// Constructs a <see cref="IdSpecification"/> specification matching any of the given <paramref name="ids"/>.
		/// </summary>
		/// <param name="ids">The IDs.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="ids"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="ids"/> is empty.</exception>
		public static IdSpecification IsAny(IEnumerable<int> ids)
		{
			// validate arguments
			if (ids == null)
				throw new ArgumentNullException("ids");

			// create the specification
			return new IdSpecification(ids);
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
			builder.Append("id=").Append(string.Join(",", IDs));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the IDs.
		/// </summary>
		public IEnumerable<int> IDs { get; private set; }
		#endregion
	}
}