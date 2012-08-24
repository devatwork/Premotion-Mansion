using System;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries.Specifications.Nodes
{
	/// <summary>
	/// Specifies a child relation.
	/// </summary>
	public class ChildOfSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs this specification.
		/// </summary>
		/// <param name="parentPointer">The pointer of the parent.</param>
		/// <param name="depth">The depth from which to select.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="parentPointer"/> is null.</exception>
		private ChildOfSpecification(NodePointer parentPointer, int? depth = 1)
		{
			// validate argument
			if (parentPointer == null)
				throw new ArgumentNullException("parentPointer");

			// set values
			ParentPointer = parentPointer;
			Depth = depth;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs a <see cref="ChildOfSpecification"/> specification matching the given <paramref name="parentPointer"/> and <paramref name="depth"/>.
		/// </summary>
		/// <param name="parentPointer">The pointer of the parent.</param>
		/// <param name="depth">The depth from which to select.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="parentPointer"/> is null.</exception>
		public static ChildOfSpecification Parent(NodePointer parentPointer, int? depth = 1)
		{
			// validate arguments
			if (parentPointer == null)
				throw new ArgumentNullException("parentPointer");

			return new ChildOfSpecification(parentPointer, depth);
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
			builder.Append("child-of=").Append(ParentPointer).Append('&').Append(Depth.HasValue ? Depth.ToString() : "any");
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="NodePointer"/> to the parent from which to select the children.
		/// </summary>
		public NodePointer ParentPointer { get; private set; }
		/// <summary>
		/// Get the depth of the childeren.
		/// Positive values are relative to the parent.
		/// Negative values are relative from the root.
		/// When value is null there is no depth specification.
		/// </summary>
		public int? Depth { get; private set; }
		#endregion
	}
}