using System;

namespace Premotion.Mansion.Core.Data.Listeners
{
	/// <summary>
	/// Implements the base class for <see cref="NodeListener"/>.
	/// </summary>
	public abstract class NodeListener : RecordListener
	{
		#region Implementation of INodeListener
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
		#endregion
		#region Template Methods
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
		#endregion
	}
}