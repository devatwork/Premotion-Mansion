using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Implements a set of <see cref="Node"/>s.
	/// </summary>
	public class Nodeset : RecordSet
	{
		#region Constructors
		/// <summary>
		/// Creates a filled nodeset.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties"></param>
		/// <param name="nodes"></param>
		public Nodeset(IMansionContext context, IPropertyBag properties, IEnumerable<Node> nodes) : base(context, properties, nodes)
		{
		}
		#endregion
		#region Row Methods
		/// <summary>
		/// Adds a new row to this set.
		/// </summary>
		/// <param name="row">The row which to add.</param>
		public override void AddRow(IPropertyBag row)
		{
			// validate arguments
			if (row == null)
				throw new ArgumentNullException("row");

			// make sure the row is a node
			if (! (row is Node))
				throw new InvalidOperationException("The row must be a node.");

			RowCollection.Add(row);
			Set("count", RowCollection.Count);
		}
		/// <summary>
		/// Removes a row from this set.
		/// </summary>
		/// <param name="row">The row which to remove.</param>
		public override void RemoveRow(IPropertyBag row)
		{
			// validate arguments
			if (row == null)
				throw new ArgumentNullException("row");

			// make sure the row is a node
			if (! (row is Node))
				throw new InvalidOperationException("The row must be a node.");

			RowCollection.Remove(row);
			Set("count", RowCollection.Count);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the nodes in this set.
		/// </summary>
		public IEnumerable<Node> Nodes
		{
			get { return RowCollection.Select(x => (Node) x); }
		}
		#endregion
	}
}