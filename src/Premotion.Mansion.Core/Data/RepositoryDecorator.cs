using System;

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
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		protected override Node DoRetrieveSingle(MansionContext context, NodeQuery query)
		{
			return DecoratedRepository.RetrieveSingle(context, query);
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected override Nodeset DoRetrieve(MansionContext context, NodeQuery query)
		{
			return DecoratedRepository.Retrieve(context, query);
		}
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		protected override Node DoCreate(MansionContext context, Node parent, IPropertyBag newProperties)
		{
			return DecoratedRepository.Create(context, parent, newProperties);
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected override void DoUpdate(MansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			DecoratedRepository.Update(context, node, modifiedProperties);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		protected override void DoDelete(MansionContext context, NodePointer pointer)
		{
			DecoratedRepository.Delete(context, pointer);
		}
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		protected override Node DoMove(MansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			return DecoratedRepository.Move(context, pointer, newParentPointer);
		}
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		protected override Node DoCopy(MansionContext context, NodePointer pointer, NodePointer targetParentPointer)
		{
			return DecoratedRepository.Copy(context, pointer, targetParentPointer);
		}
		/// <summary>
		/// Parses <paramref name="arguments" /> into a <see cref="NodeQuery" />.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns the parsed query.</returns>
		protected override NodeQuery DoParseQuery(MansionContext context, IPropertyBag arguments)
		{
			return DecoratedRepository.ParseQuery(context, arguments);
		}
		/// <summary>
		/// Starts this object. This methods must be called after the object has been created and before it is used.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoStart(MansionContext context)
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