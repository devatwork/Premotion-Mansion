using System;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Implements a node.
	/// </summary>
	public sealed class Node : PropertyBag
	{
		#region Constructors
		/// <summary>
		/// Constructs an empty node.
		/// </summary>
		public Node()
		{
		}
		/// <summary>
		/// Constructs a new node with the specified pointer.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node.</param>
		/// <param name="extendedProperties"></param>
		public Node(IMansionContext context, NodePointer pointer, IPropertyBag extendedProperties) : base(extendedProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (extendedProperties == null)
				throw new ArgumentNullException("extendedProperties");

			// set values
			this.pointer = pointer;

			// copy the pointer properties to the property bag
			Set("id", Pointer.Id);
			Set("pointer", Pointer.PointerString);
			if (Pointer.HasParent)
			{
				Set("parentPointer", Pointer.Parent.PointerString);
				Set("parentId", Pointer.Parent.Id);
			}
			Set("path", Pointer.PathString);
			Set("structure", Pointer.StructureString);
			Set("name", Pointer.Name);
			Set("type", Pointer.Type);
			if (Pointer.HasParent)
				Set("parentPointer", Pointer.Parent.PointerString);

			// determine the state of the node
			var isApproved = extendedProperties.Get<bool>(context, "approved");
			var publicationDate = extendedProperties.Get<DateTime>(context, "publicationDate");
			var expirationDate = extendedProperties.Get<DateTime>(context, "expirationDate");
			var isArchived = extendedProperties.Get<bool>(context, "archived");
			if (isArchived)
			{
				status = NodeStatus.Archived;
				Set("status", "archived");
			}
			else if (!isApproved)
			{
				status = NodeStatus.Draft;
				Set("status", "draft");
			}
			else if (expirationDate < DateTime.Now)
			{
				status = NodeStatus.Expired;
				Set("status", "expired");
			}
			else if (publicationDate > DateTime.Now)
			{
				status = NodeStatus.Staged;
				Set("status", "staged");
			}
			else
			{
				status = NodeStatus.Published;
				Set("status", "published");
			}

			// set misc properties
			order = extendedProperties.Get<long>(context, "order");
			guid = extendedProperties.Get<Guid>(context, "guid");
		}
		#endregion
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