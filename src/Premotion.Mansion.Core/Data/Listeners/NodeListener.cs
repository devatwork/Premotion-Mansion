using System;

namespace Premotion.Mansion.Core.Data.Listeners
{
	/// <summary>
	/// Implements the base class for <see cref="NodeListener"/>.
	/// </summary>
	public abstract class NodeListener
	{
		#region Implementation of INodeListener
		/// <summary>
		/// This method is called just before a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child will be added.</param>
		/// <param name="newProperties">The new properties of the node.</param>
		public void BeforeCreate(MansionContext context, Node parent, IPropertyBag newProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (newProperties == null)
				throw new ArgumentNullException("newProperties");

			// invoke template method
			DoBeforeCreate(context, parent, newProperties);
		}
		/// <summary>
		/// This method is called just after a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child was be added.</param>
		/// <param name="node">The created node.</param>
		/// <param name="newProperties">The properties from which the <paramref name="node"/> was constructed.</param>
		public void AfterCreate(MansionContext context, Node parent, Node node, IPropertyBag newProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (node == null)
				throw new ArgumentNullException("node");
			if (newProperties == null)
				throw new ArgumentNullException("newProperties");

			// invoke template method
			DoAfterCreate(context, parent, node, newProperties);
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which will be modified.</param>
		/// <param name="modifiedProperties">The updated properties of the node.</param>
		public void BeforeUpdate(MansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");

			// invoke template method
			DoBeforeUpdate(context, node, modifiedProperties);
		}
		/// <summary>
		/// This method is called just after a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The modified node.</param>
		/// <param name="modifiedProperties">The properties which were set to the updated <paramref name="node"/>.</param>
		public void AfterUpdate(MansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");

			// invoke template method
			DoAfterUpdate(context, node, modifiedProperties);
		}
		/// <summary>
		/// This method is called just before a node is deleted from the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node being deleted.</param>
		public void BeforeDelete(MansionContext context, NodePointer pointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");

			// invoke template method
			DoBeforeDelete(context, pointer);
		}
		/// <summary>
		/// This method is called just before a node is moved in the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node being moved.</param>
		/// <param name="newParentPointer">The pointer to the new parent.</param>
		public void BeforeMove(MansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (newParentPointer == null)
				throw new ArgumentNullException("newParentPointer");

			// invoke template method
			DoBeforeMove(context, pointer, newParentPointer);
		}
		/// <summary>
		/// This method is called just before a node is moved in the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The moved node.</param>
		public void AfterMove(MansionContext context, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			// invoke template method
			DoAfterMove(context, node);
		}
		/// <summary>
		/// This method is called just before a node is copied in the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node being copied.</param>
		/// <param name="newParentPointer">The pointer to the parent under which the node is copied.</param>
		public void BeforeCopy(MansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (newParentPointer == null)
				throw new ArgumentNullException("newParentPointer");

			// invoke template method
			DoBeforeCopy(context, pointer, newParentPointer);
		}
		/// <summary>
		/// This method is called just before a node is copied in the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The copied node.</param>
		public void AfterCopy(MansionContext context, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			// invoke template method
			DoAfterCopy(context, node);
		}
		/// <summary>
		/// This method is called when an property which is not on the node is accessed. Useful for lazy loading properties.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> which does not have the property..</param>
		/// <param name="propertyName">The name of the property being retrieved.</param>
		/// <param name="value">The missing value</param>
		public bool TryResolveMissingProperty(MansionContext context, Node node, string propertyName, out object value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");
			if (propertyName == null)
				throw new ArgumentNullException("propertyName");

			// invoke template method
			return DoTryResolveMissingProperty(context, node, propertyName, out value);
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// This method is called just before a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child will be added.</param>
		/// <param name="newProperties">The new properties of the node.</param>
		protected virtual void DoBeforeCreate(MansionContext context, Node parent, IPropertyBag newProperties)
		{
		}
		/// <summary>
		/// This method is called just after a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child was be added.</param>
		/// <param name="node">The created node.</param>
		/// <param name="newProperties">The properties from which the <paramref name="node"/> was constructed.</param>
		protected virtual void DoAfterCreate(MansionContext context, Node parent, Node node, IPropertyBag newProperties)
		{
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which will be modified.</param>
		/// <param name="modifiedProperties">The updated properties of the node.</param>
		protected virtual void DoBeforeUpdate(MansionContext context, Node node, IPropertyBag modifiedProperties)
		{
		}
		/// <summary>
		/// This method is called just after a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The modified node.</param>
		/// <param name="modifiedProperties">The properties which were set to the updated <paramref name="node"/>.</param>
		protected virtual void DoAfterUpdate(MansionContext context, Node node, IPropertyBag modifiedProperties)
		{
		}
		/// <summary>
		/// This method is called just before a node is deleted from the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node being deleted.</param>
		protected virtual void DoBeforeDelete(MansionContext context, NodePointer pointer)
		{
		}
		/// <summary>
		/// This method is called just before a node is moved in the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node being moved.</param>
		/// <param name="newParentPointer">The pointer to the new parent.</param>
		protected virtual void DoBeforeMove(MansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
		}
		/// <summary>
		/// This method is called just before a node is moved in the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The moved node.</param>
		protected virtual void DoAfterMove(MansionContext context, Node node)
		{
		}
		/// <summary>
		/// This method is called just before a node is copied in the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node being copied.</param>
		/// <param name="newParentPointer">The pointer to the parent under which the node is copied.</param>
		protected virtual void DoBeforeCopy(MansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
		}
		/// <summary>
		/// This method is called just before a node is copied in the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The copied node.</param>
		protected virtual void DoAfterCopy(MansionContext context, Node node)
		{
		}
		/// <summary>
		/// This method is called when an property which is not on the node is accessed. Useful for lazy loading properties.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> which does not have the property..</param>
		/// <param name="propertyName">The name of the property being retrieved.</param>
		/// <param name="value">The missing value</param>
		protected virtual bool DoTryResolveMissingProperty(MansionContext context, Node node, string propertyName, out object value)
		{
			value = null;
			return false;
		}
		#endregion
	}
}