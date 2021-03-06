﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries.Specifications.Nodes
{
	/// <summary>
	/// Specifies a parent relation.
	/// </summary>
	public class ParentOfSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs this specification.
		/// </summary>
		/// <param name="childPointer">The pointer of the parent.</param>
		/// <param name="depth">The depth from which to select.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="childPointer"/> is null.</exception>
		private ParentOfSpecification(NodePointer childPointer, int? depth = 1)
		{
			// validate argument
			if (childPointer == null)
				throw new ArgumentNullException("childPointer");

			// set values
			ChildPointer = childPointer;
			Depth = depth;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs a <see cref="ParentOfSpecification"/> specification matching the given <paramref name="childPointer"/> and <paramref name="depth"/>.
		/// </summary>
		/// <param name="childPointer">The pointer of the parent.</param>
		/// <param name="depth">The depth from which to select.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="childPointer"/> is null.</exception>
		public static ParentOfSpecification Child(NodePointer childPointer, int? depth = 1)
		{
			// validate arguments
			if (childPointer == null)
				throw new ArgumentNullException("childPointer");

			return new ParentOfSpecification(childPointer, depth);
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Gets the names of the properties used by this query component.
		/// </summary>
		/// <returns>Returns the property hints.</returns>
		protected override IEnumerable<string> DoGetPropertyHints()
		{
			return new[] {"pointer"};
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append("parent-of=").Append(ChildPointer).Append('&').Append(Depth.HasValue ? Depth.ToString() : "any");
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="NodePointer"/> to the parent from which to select the children.
		/// </summary>
		public NodePointer ChildPointer { get; private set; }
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