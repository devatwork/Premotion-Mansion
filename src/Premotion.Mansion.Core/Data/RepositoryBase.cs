using System;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Defines the base class for repositories.
	/// </summary>
	public abstract class RepositoryBase : DisposableBase, IRepository
	{
		#region Implementation of IRepository
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns the node.</returns>
		public Node RetrieveSingle(MansionContext context, NodeQuery query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");
			CheckDisposed();

			// invoke template method
			return DoRetrieveSingle(context, query);
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns></returns>
		public Nodeset Retrieve(MansionContext context, NodeQuery query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");
			CheckDisposed();

			// invoke template method
			return DoRetrieve(context, query);
		}
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		public Node Create(MansionContext context, Node parent, IPropertyBag newProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (newProperties == null)
				throw new ArgumentNullException("newProperties");
			CheckDisposed();

			// if no allowedRoleGuids are specified, copy the parens
			if (!newProperties.Contains("allowedRoleGuids"))
				newProperties.Set("allowedRoleGuids", parent.Get(context, "allowedRoleGuids", string.Empty));

			// invoke template method
			return DoCreate(context, parent, newProperties);
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		public void Update(MansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");
			CheckDisposed();

			// invoke template method
			DoUpdate(context, node, modifiedProperties);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		public void Delete(MansionContext context, NodePointer pointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			CheckDisposed();

			// invoke template method
			DoDelete(context, pointer);
		}
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		public Node Move(MansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (newParentPointer == null)
				throw new ArgumentNullException("newParentPointer");
			CheckDisposed();

			// invoke template method
			return DoMove(context, pointer, newParentPointer);
		}
		/// <summary>
		/// Parses <paramref name="arguments" /> into a <see cref="NodeQuery" />.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns the parsed query.</returns>
		public NodeQuery ParseQuery(MansionContext context, IPropertyBag arguments)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (arguments == null)
				throw new ArgumentNullException("arguments");
			CheckDisposed();

			// invoke template method
			return DoParseQuery(context, arguments);
		}
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		public Node Copy(MansionContext context, NodePointer pointer, NodePointer targetParentPointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (targetParentPointer == null)
				throw new ArgumentNullException("targetParentPointer");
			CheckDisposed();

			// invoke template method
			return DoCopy(context, pointer, targetParentPointer);
		}
		#endregion
		#region Implementation of IStartable
		/// <summary>
		/// Starts this object. This methods must be called after the object has been created and before it is used.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		public void Start(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			DoStart(context);
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Starts this object. This methods must be called after the object has been created and before it is used.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected abstract void DoStart(MansionContext context);
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns the node.</returns>
		protected abstract Node DoRetrieveSingle(MansionContext context, NodeQuery query);
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns></returns>
		protected abstract Nodeset DoRetrieve(MansionContext context, NodeQuery query);
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		protected abstract Node DoCreate(MansionContext context, Node parent, IPropertyBag newProperties);
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected abstract void DoUpdate(MansionContext context, Node node, IPropertyBag modifiedProperties);
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		protected abstract void DoDelete(MansionContext context, NodePointer pointer);
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		protected abstract Node DoMove(MansionContext context, NodePointer pointer, NodePointer newParentPointer);
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		protected abstract Node DoCopy(MansionContext context, NodePointer pointer, NodePointer newParentPointer);
		/// <summary>
		/// Parses <paramref name="arguments" /> into a <see cref="NodeQuery" />.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns the parsed query.</returns>
		protected abstract NodeQuery DoParseQuery(MansionContext context, IPropertyBag arguments);
		#endregion
	}
}