using System;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Represents an engine which can store data.
	/// </summary>
	public abstract class BaseStorageEngine
	{
		#region Record Methods
		/// <summary>
		/// Creates a new record with the given <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties from which to create a record.</param>
		/// <returns>Returns the created <see cref="Record"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="properties"/> is null.</exception>
		public Record Create(IMansionContext context, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			return DoCreate(context, properties);
		}
		/// <summary>
		/// Creates a new record with the given <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties from which to create a record.</param>
		/// <returns>Returns the created <see cref="Record"/>.</returns>
		protected abstract Record DoCreate(IMansionContext context, IPropertyBag properties);
		/// <summary>
		/// Updates an existing <paramref name="record"/> in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be updated.</param>
		/// <param name="properties">The updated properties.</param>
		/// <returns>Returns the updated <see cref="Record"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="record"/> or <paramref name="properties"/> is null.</exception>
		public Record Update(IMansionContext context, Record record, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			return DoUpdate(context, record, properties);
		}
		/// <summary>
		/// Updates an existing <paramref name="record"/> in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be updated.</param>
		/// <param name="properties">The updated properties.</param>
		protected abstract Record DoUpdate(IMansionContext context, Record record, IPropertyBag properties);
		/// <summary>
		/// Deletes an existing <paramref name="record"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be deleted.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="record"/> is null.</exception>
		public void Delete(IMansionContext context, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");

			// invoke template method
			DoDelete(context, record);
		}
		/// <summary>
		/// Deletes an existing <paramref name="record"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be deleted.</param>
		protected abstract void DoDelete(IMansionContext context, Record record);
		#endregion
		#region Node Methods
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="properties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="parent"/> or <paramref name="properties"/> is null.</exception>
		public Node CreateNode(IMansionContext context, Node parent, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			return DoCreateNode(context, parent, properties);
		}
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="properties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		protected abstract Node DoCreateNode(IMansionContext context, Node parent, IPropertyBag properties);
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="properties">The properties which to update.</param>
		/// <returns>Returns the updated <see cref="Node"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="node"/> or <paramref name="properties"/> is null.</exception>
		public Node UpdateNode(IMansionContext context, Node node, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			return DoUpdateNode(context, node, properties);
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="properties">The properties which to update.</param>
		protected abstract Node DoUpdateNode(IMansionContext context, Node node, IPropertyBag properties);
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The pointer to the node which will be deleted.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="node"/> is null.</exception>
		public void DeleteNode(IMansionContext context, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			// invoke template method
			DoDeleteNode(context, node);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The pointer to the node which will be deleted.</param>
		protected abstract void DoDeleteNode(IMansionContext context, Node node);
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="pointer"/> or <paramref name="newParentPointer"/> is null.</exception>
		public Node MoveNode(IMansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (newParentPointer == null)
				throw new ArgumentNullException("newParentPointer");

			// invoke template method
			return DoMoveNode(context, pointer, newParentPointer);
		}
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		protected abstract Node DoMoveNode(IMansionContext context, NodePointer pointer, NodePointer newParentPointer);
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="pointer"/> or <paramref name="targetParentPointer"/> is null.</exception>
		public Node CopyNode(IMansionContext context, NodePointer pointer, NodePointer targetParentPointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (targetParentPointer == null)
				throw new ArgumentNullException("targetParentPointer");

			// invoke template method
			return DoCopyNode(context, pointer, targetParentPointer);
		}
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		protected abstract Node DoCopyNode(IMansionContext context, NodePointer pointer, NodePointer targetParentPointer);
		#endregion
	}
}