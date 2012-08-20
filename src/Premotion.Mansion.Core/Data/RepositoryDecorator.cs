using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Base class for all repository decorator classes.
	/// </summary>
	public abstract class RepositoryDecorator : RepositoryBase, IRepositoryDecorator
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="decoratedRepository"></param>
		protected RepositoryDecorator(IRepository decoratedRepository)
		{
			// validate arguments
			if (decoratedRepository == null)
				throw new ArgumentNullException("decoratedRepository");

			// set values
			this.decoratedRepository = decoratedRepository;
		}
		#endregion
		#region Implementation of IRepository
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		protected override Node DoRetrieveSingleNode(IMansionContext context, Query query)
		{
			return DecoratedRepository.RetrieveSingleNode(context, query);
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected override Nodeset DoRetrieveNodeset(IMansionContext context, Query query)
		{
			return DecoratedRepository.RetrieveNodeset(context, query);
		}
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		protected override Node DoCreateNode(IMansionContext context, Node parent, IPropertyBag newProperties)
		{
			return DecoratedRepository.CreateNode(context, parent, newProperties);
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected override void DoUpdateNode(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			DecoratedRepository.UpdateNode(context, node, modifiedProperties);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		protected override void DoDeleteNode(IMansionContext context, NodePointer pointer)
		{
			DecoratedRepository.DeleteNode(context, pointer);
		}
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		protected override Node DoMoveNode(IMansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			return DecoratedRepository.MoveNode(context, pointer, newParentPointer);
		}
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		protected override Node DoCopyNode(IMansionContext context, NodePointer pointer, NodePointer targetParentPointer)
		{
			return DecoratedRepository.CopyNode(context, pointer, targetParentPointer);
		}
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="IPropertyBag"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override IPropertyBag DoRetrieveSingle(IMansionContext context, Query query)
		{
			return DecoratedRepository.RetrieveSingle(context, query);
		}
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override Dataset DoRetrieve(IMansionContext context, Query query)
		{
			return DecoratedRepository.Retrieve(context, query);
		}
		/// <summary>
		/// Starts this object. This methods must be called after the object has been created and before it is used.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoStart(IMansionContext context)
		{
			DecoratedRepository.Start(context);
		}
		#endregion
		#region Implementation of IRepositoryDecorator
		/// <summary>
		/// Gets the <see cref="IRepository"/> being decorated.
		/// </summary>
		public IRepository DecoratedRepository
		{
			get { return decoratedRepository; }
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
			if (!disposeManagedResources)
				return;

			// execute the decorated object
			decoratedRepository.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly IRepository decoratedRepository;
		#endregion
	}
}