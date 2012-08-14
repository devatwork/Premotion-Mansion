using System;
using System.Linq;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Core.Data.Caching
{
	/// <summary>
	/// Decorates any <see cref="IRepository"/> in order to add the caching function to any existing repository.
	/// </summary>
	public class CachingRepositoryDecorator : RepositoryDecorator
	{
		#region Constructors
		/// <summary>
		/// Constructs a caching decorated <see cref="IRepository"/>.
		/// </summary>
		/// <param name="decoratedRepository">The <see cref="IRepository"/> being decorated.</param>
		/// <param name="cachingService">The <see cref="ICachingService"/>service which to use.</param>
		public CachingRepositoryDecorator(IRepository decoratedRepository, ICachingService cachingService) : base(decoratedRepository)
		{
			// validate arguments
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// set values.
			this.cachingService = cachingService;
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
			// check if this query is not cachable
			if (query.Components.Any(candidate => candidate is CacheQueryComponent && !((CacheQueryComponent) candidate).IsEnabled))
				return DecoratedRepository.RetrieveSingleNode(context, query);

			// create the cache key for this node
			var nodeCacheKey = NodeCacheKeyFactory.CreateForNode(query);

			// return the node
			return cachingService.GetOrAdd(context, nodeCacheKey, () => new CachedNode(DecoratedRepository.RetrieveSingleNode(context, query)));
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected override Nodeset DoRetrieveNodeset(IMansionContext context, Query query)
		{
			// check if this query is not cachable
			if (query.Components.Any(candidate => candidate is CacheQueryComponent && !((CacheQueryComponent) candidate).IsEnabled))
				return DecoratedRepository.RetrieveNodeset(context, query);

			// create the cache key for this node
			var nodesetCacheKey = NodeCacheKeyFactory.CreateForNodeset(query);

			// return the node
			return cachingService.GetOrAdd(context, nodesetCacheKey, () => new CachedNodeset(DecoratedRepository.RetrieveNodeset(context, query)));
		}
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		protected override Node DoCreate(IMansionContext context, Node parent, IPropertyBag newProperties)
		{
			// excute derived class
			var node = DecoratedRepository.Create(context, parent, newProperties);

			// clear all cached nodes and nodesets
			cachingService.Clear(NodeCacheKeyFactory.RepositoryModifiedDependency.Key);

			return node;
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected override void DoUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// excute derived class
			DecoratedRepository.Update(context, node, modifiedProperties);

			// clear all cached nodes and nodesets
			cachingService.Clear(NodeCacheKeyFactory.RepositoryModifiedDependency.Key);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		protected override void DoDelete(IMansionContext context, NodePointer pointer)
		{
			// excute derived class
			DecoratedRepository.Delete(context, pointer);

			// clear all cached nodes and nodesets
			cachingService.Clear(NodeCacheKeyFactory.RepositoryModifiedDependency.Key);
		}
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		protected override Node DoMove(IMansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			// excute derived class
			var node = DecoratedRepository.Move(context, pointer, newParentPointer);

			// clear all cached nodes and nodesets
			cachingService.Clear(NodeCacheKeyFactory.RepositoryModifiedDependency.Key);

			return node;
		}
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		protected override Node DoCopy(IMansionContext context, NodePointer pointer, NodePointer targetParentPointer)
		{
			// excute derived class
			var node = DecoratedRepository.Copy(context, pointer, targetParentPointer);

			// clear all cached nodes and nodesets
			cachingService.Clear(NodeCacheKeyFactory.RepositoryModifiedDependency.Key);

			return node;
		}
		#endregion
		#region Private Fields
		private readonly ICachingService cachingService;
		#endregion
	}
}