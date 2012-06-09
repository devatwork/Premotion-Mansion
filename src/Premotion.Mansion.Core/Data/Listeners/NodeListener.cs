﻿using System;

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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child will be added.</param>
		/// <param name="newProperties">The new properties of the node.</param>
		public void BeforeCreate(IMansionContext context, Node parent, IPropertyBag newProperties)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child was be added.</param>
		/// <param name="node">The created node.</param>
		/// <param name="newProperties">The properties from which the <paramref name="node"/> was constructed.</param>
		public void AfterCreate(IMansionContext context, Node parent, Node node, IPropertyBag newProperties)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be modified.</param>
		/// <param name="modifiedProperties">The updated properties of the node.</param>
		public void BeforeUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The modified node.</param>
		/// <param name="modifiedProperties">The properties which were set to the updated <paramref name="node"/>.</param>
		public void AfterUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node being deleted.</param>
		public void BeforeDelete(IMansionContext context, NodePointer pointer)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="originalParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> from which <paramref name="pointer"/> is being moved.</param>
		/// <param name="targetParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> to which <paramref name="pointer"/> is being moved.</param>
		/// <param name="pointer">The pointer to the node being moved.</param>
		public void BeforeMove(IMansionContext context, NodePointer originalParentPointer, NodePointer targetParentPointer, NodePointer pointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (originalParentPointer == null)
				throw new ArgumentNullException("originalParentPointer");
			if (targetParentPointer == null)
				throw new ArgumentNullException("targetParentPointer");
			if (pointer == null)
				throw new ArgumentNullException("pointer");

			// invoke template method
			DoBeforeMove(context, originalParentPointer, targetParentPointer, pointer);
		}
		/// <summary>
		/// This method is called just before a node is moved in the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="originalParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> from which <paramref name="node"/> was moved.</param>
		/// <param name="targetParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> to which <paramref name="node"/> was moved.</param>
		/// <param name="node">The moved node.</param>
		public void AfterMove(IMansionContext context, NodePointer originalParentPointer, NodePointer targetParentPointer, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (originalParentPointer == null)
				throw new ArgumentNullException("originalParentPointer");
			if (targetParentPointer == null)
				throw new ArgumentNullException("targetParentPointer");
			if (node == null)
				throw new ArgumentNullException("node");

			// invoke template method
			DoAfterMove(context, originalParentPointer, targetParentPointer, node);
		}
		/// <summary>
		/// This method is called just before a node is copied in the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="originalParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> from which <paramref name="pointer"/> is being copied.</param>
		/// <param name="targetParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> to which <paramref name="pointer"/> is being copied.</param>
		/// <param name="pointer">The pointer to the node being copied.</param>
		public void BeforeCopy(IMansionContext context, NodePointer originalParentPointer, NodePointer targetParentPointer, NodePointer pointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (originalParentPointer == null)
				throw new ArgumentNullException("originalParentPointer");
			if (targetParentPointer == null)
				throw new ArgumentNullException("targetParentPointer");
			if (pointer == null)
				throw new ArgumentNullException("pointer");

			// invoke template method
			DoBeforeCopy(context, originalParentPointer, targetParentPointer, pointer);
		}
		/// <summary>
		/// This method is called just before a node is copied in the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="originalParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> from which <paramref name="node"/> was copied.</param>
		/// <param name="targetParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> to which <paramref name="node"/> was copied.</param>
		/// <param name="node">The copied node.</param>
		public void AfterCopy(IMansionContext context, NodePointer originalParentPointer, NodePointer targetParentPointer, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (originalParentPointer == null)
				throw new ArgumentNullException("originalParentPointer");
			if (targetParentPointer == null)
				throw new ArgumentNullException("targetParentPointer");
			if (node == null)
				throw new ArgumentNullException("node");

			// invoke template method
			DoAfterCopy(context, originalParentPointer, targetParentPointer, node);
		}
		/// <summary>
		/// This method is called when an property which is not on the node is accessed. Useful for lazy loading properties.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> which does not have the property..</param>
		/// <param name="propertyName">The name of the property being retrieved.</param>
		/// <param name="value">The missing value</param>
		public bool TryResolveMissingProperty(IMansionContext context, Node node, string propertyName, out object value)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child will be added.</param>
		/// <param name="newProperties">The new properties of the node.</param>
		protected virtual void DoBeforeCreate(IMansionContext context, Node parent, IPropertyBag newProperties)
		{
		}
		/// <summary>
		/// This method is called just after a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child was be added.</param>
		/// <param name="node">The created node.</param>
		/// <param name="newProperties">The properties from which the <paramref name="node"/> was constructed.</param>
		protected virtual void DoAfterCreate(IMansionContext context, Node parent, Node node, IPropertyBag newProperties)
		{
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be modified.</param>
		/// <param name="modifiedProperties">The updated properties of the node.</param>
		protected virtual void DoBeforeUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
		}
		/// <summary>
		/// This method is called just after a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The modified node.</param>
		/// <param name="modifiedProperties">The properties which were set to the updated <paramref name="node"/>.</param>
		protected virtual void DoAfterUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
		}
		/// <summary>
		/// This method is called just before a node is deleted from the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node being deleted.</param>
		protected virtual void DoBeforeDelete(IMansionContext context, NodePointer pointer)
		{
		}
		/// <summary>
		/// This method is called just before a node is moved in the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="originalParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> from which <paramref name="pointer"/> is being moved.</param>
		/// <param name="targetParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> to which <paramref name="pointer"/> is being moved.</param>
		/// <param name="pointer">The pointer to the node being moved.</param>
		protected virtual void DoBeforeMove(IMansionContext context, NodePointer originalParentPointer, NodePointer targetParentPointer, NodePointer pointer)
		{
		}
		/// <summary>
		/// This method is called just before a node is moved in the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="originalParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> from which <paramref name="node"/> was moved.</param>
		/// <param name="targetParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> to which <paramref name="node"/> was moved.</param>
		/// <param name="node">The moved node.</param>
		protected virtual void DoAfterMove(IMansionContext context, NodePointer originalParentPointer, NodePointer targetParentPointer, Node node)
		{
		}
		/// <summary>
		/// This method is called just before a node is copied in the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="originalParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> from which <paramref name="pointer"/> is being copied.</param>
		/// <param name="targetParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> to which <paramref name="pointer"/> is being copied.</param>
		/// <param name="pointer">The pointer to the node being copied.</param>
		protected virtual void DoBeforeCopy(IMansionContext context, NodePointer originalParentPointer, NodePointer targetParentPointer, NodePointer pointer)
		{
		}
		/// <summary>
		/// This method is called just before a node is copied in the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="originalParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> from which <paramref name="node"/> was copied.</param>
		/// <param name="targetParentPointer">The <see cref="NodePointer"/> of the parent <see cref="Node"/> to which <paramref name="node"/> was copied.</param>
		/// <param name="node">The copied node.</param>
		protected virtual void DoAfterCopy(IMansionContext context, NodePointer originalParentPointer, NodePointer targetParentPointer, Node node)
		{
		}
		/// <summary>
		/// This method is called when an property which is not on the node is accessed. Useful for lazy loading properties.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> which does not have the property..</param>
		/// <param name="propertyName">The name of the property being retrieved.</param>
		/// <param name="value">The missing value</param>
		protected virtual bool DoTryResolveMissingProperty(IMansionContext context, Node node, string propertyName, out object value)
		{
			value = null;
			return false;
		}
		#endregion
	}
}