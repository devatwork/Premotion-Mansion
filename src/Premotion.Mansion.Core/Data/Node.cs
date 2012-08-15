using System;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Implements a node.
	/// </summary>
	public sealed class Node : PropertyBag
	{
		#region Initialize Methods
		/// <summary>
		/// Initializes this node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/></param>
		public void Initialize(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// set values
			pointer = Get<NodePointer>(context, "pointer");

			// determine the state of the node
			var isApproved = Get<bool>(context, "approved");
			var publicationDate = Get<DateTime>(context, "publicationDate");
			var expirationDate = Get<DateTime>(context, "expirationDate");
			var isArchived = Get<bool>(context, "archived");
			if (isArchived)
				status = NodeStatus.Archived;
			else if (!isApproved)
				status = NodeStatus.Draft;
			else if (expirationDate < DateTime.Now)
				status = NodeStatus.Expired;
			else if (publicationDate > DateTime.Now)
				status = NodeStatus.Staged;
			else
				status = NodeStatus.Published;

			// set misc properties
			order = Get<long>(context, "order");
			guid = Get<Guid>(context, "guid");
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the pointer of this node.
		/// </summary>
		public NodePointer Pointer
		{
			get { return pointer; }
		}
		/// <summary>
		/// Gets the <see cref="NodeStatus"/> of this node.
		/// </summary>
		public NodeStatus Status
		{
			get { return status; }
		}
		/// <summary>
		/// Gets the order of this Node.
		/// </summary>
		public long Order
		{
			get { return order; }
		}
		/// <summary>
		/// Gets the order of this Node.
		/// </summary>
		public Guid PermanentId
		{
			get { return guid; }
		}
		#endregion
		#region Private Fields
		private Guid guid;
		private long order;
		private NodePointer pointer;
		private NodeStatus status;
		#endregion
	}
}