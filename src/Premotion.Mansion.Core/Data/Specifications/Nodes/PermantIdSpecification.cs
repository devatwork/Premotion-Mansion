using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Data.Specifications.Nodes
{
	/// <summary>
	/// Specifies PermantIds.
	/// </summary>
	public class PermantIdSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs this query part with the specified base type.
		/// </summary>
		/// <param name="permantIds">The types.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="permantIds"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="permantIds"/> is empty.</exception>
		private PermantIdSpecification(IEnumerable<Guid> permantIds)
		{
			// validate arguments
			if (permantIds == null)
				throw new ArgumentNullException("permantIds");

			// guard against empty types
			var permantIdArray = permantIds.ToArray();
			if (permantIdArray.Length == 0)
				throw new InvalidOperationException("The PermantIdSpecification should at least include one PermantId");

			// set value
			PermantIds = permantIdArray;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs a <see cref="PermantIdSpecification"/> specification matching the given <paramref name="permantId"/>.
		/// </summary>
		/// <param name="permantId">The PermantId.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="permantId"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="permantId"/> is empty.</exception>
		public static PermantIdSpecification Is(Guid permantId)
		{
			return IsAny(permantId);
		}
		/// <summary>
		/// Constructs a <see cref="PermantIdSpecification"/> specification matching any of the given <paramref name="permantIds"/>.
		/// </summary>
		/// <param name="permantIds">The PermantIds.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="permantIds"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="permantIds"/> is empty.</exception>
		public static PermantIdSpecification IsAny(params Guid[] permantIds)
		{
			return new PermantIdSpecification(permantIds);
		}
		/// <summary>
		/// Constructs a <see cref="PermantIdSpecification"/> specification matching any of the given <paramref name="permantIds"/>.
		/// </summary>
		/// <param name="permantIds">The PermantIds.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="permantIds"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="permantIds"/> is empty.</exception>
		public static PermantIdSpecification IsAny(IEnumerable<Guid> permantIds)
		{
			// valPermantIdate arguments
			if (permantIds == null)
				throw new ArgumentNullException("permantIds");

			// create the specification
			return new PermantIdSpecification(permantIds);
		}
		#endregion
		#region OverrPermantIdes of Specification
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append("permant-ids=").Append(string.Join(",", PermantIds));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the PermantIds.
		/// </summary>
		public IEnumerable<Guid> PermantIds { get; private set; }
		#endregion
	}
}