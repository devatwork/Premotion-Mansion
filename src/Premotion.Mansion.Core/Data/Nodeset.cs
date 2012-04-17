using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Implements a set of <see cref="Node"/>s.
	/// </summary>
	public class Nodeset : Dataset
	{
		#region Constructors
		/// <summary>
		/// Creates a filled nodeset.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="nodes"></param>
		/// <param name="properties"></param>
		public Nodeset(IMansionContext context, IEnumerable<Node> nodes, IPropertyBag properties) : base(properties)
		{
			// validate arguments
			if (nodes == null)
				throw new ArgumentNullException("nodes");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// set values
			foreach (var node in nodes)
				RowCollection.Add(node);

			// check for paging
			var totalRowCount = properties.Get(context, "totalCount", -1);
			var pageNumber = properties.Get(context, "pageNumber", -1);
			var rowsPerPage = properties.Get(context, "pageSize", -1);
			if (totalRowCount != -1 && pageNumber != -1 && rowsPerPage != -1)
				SetPaging(totalRowCount, pageNumber, rowsPerPage);

			// check for sort
			string sortString;
			if (properties.TryGet(context, "sort", out sortString))
			{
				foreach (var sort in Collections.Sort.Parse(sortString))
					AddSort(sort);
			}
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