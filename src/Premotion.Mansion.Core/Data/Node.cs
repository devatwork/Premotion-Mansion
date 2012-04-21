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
		private readonly Guid guid;
		private readonly long order;
		private readonly NodePointer pointer;
		private readonly NodeStatus status;
		#endregion
	}
}