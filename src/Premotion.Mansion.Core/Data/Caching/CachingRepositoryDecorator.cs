using System;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Core.Data.Caching
{
	/// <summary>
	/// Decorates any <see cref="IRepository"/> in order to add the caching function to any existing repository.
	/// </summary>
	public class CachingRepositoryDecorator : RepositoryDecorator
	{
		#region Constants
		/// <summary>
		/// Defines the cache key prefix.
		/// </summary>
		private const string RepositoryCacheKeyPrefix = "Repository_";
		/// <summary>
		/// Defines the repository modified cache key.
		/// </summary>
		public static readonly StringCacheKeyDependency RepositoryModifiedDependency = new StringCacheKeyDependency(RepositoryCacheKeyPrefix + "Modified");
		#endregion
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
			// check if this query is not cacheable
			if (!query.Iscacheable())
				return DecoratedRepository.RetrieveSingleNode(context, query);

			// create the cache key for this node
			var cacheKey = query.CalculateCacheKey("Node_Query_");

			// return the node
			return cachingService.GetOrAdd(context, cacheKey, () => DecoratedRepository.RetrieveSingleNode(context, query).AsCacheableObject());
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected override Nodeset DoRetrieveNodeset(IMansionContext context, Query query)
		{
			// check if this query is not cacheable
			if (!query.Iscacheable())
				return DecoratedRepository.RetrieveNodeset(context, query);

			// create the cache key for this node
			var cacheKey = query.CalculateCacheKey("Nodeset_Query_");

			// return the node
			return cachingService.GetOrAdd(context, cacheKey, () => DecoratedRepository.RetrieveNodeset(context, query).AsCacheableObject());
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
			// excute derived class
			var node = DecoratedRepository.CreateNode(context, parent, newProperties);

			// clear the cache for the given node
			node.ClearFromCache(cachingService);

			return node;
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected override void DoUpdateNode(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// excute derived class
			DecoratedRepository.UpdateNode(context, node, modifiedProperties);

			// clear the cache for the given node
			node.ClearFromCache(cachingService);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		protected override void DoDeleteNode(IMansionContext context, NodePointer pointer)
		{
			// excute derived class
			DecoratedRepository.DeleteNode(context, pointer);

			// clear all cached nodes and nodesets
			// TODO: refactor this method to always use a node
			cachingService.Clear(RepositoryModifiedDependency.Key);
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
			// excute derived class
			var node = DecoratedRepository.MoveNode(context, pointer, newParentPointer);

			// clear the cache for the given node
			node.ClearFromCache(cachingService);

			return node;
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
			// excute derived class
			var node = DecoratedRepository.CopyNode(context, pointer, targetParentPointer);

			// clear the cache for the given node
			node.ClearFromCache(cachingService);

			return node;
		}
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Record"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override Record DoRetrieveSingle(IMansionContext context, Query query)
		{
			// check if this query is not cacheable
			if (!query.Iscacheable())
				return DecoratedRepository.RetrieveSingle(context, query);

			// create the cache key for this node
			var cacheKey = query.CalculateCacheKey("Record_Query_");

			// return the node
			return cachingService.GetOrAdd(context, cacheKey, () => DecoratedRepository.RetrieveSingle(context, query).AsCacheableObject());
		}
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override RecordSet DoRetrieve(IMansionContext context, Query query)
		{
			// check if this query is not cacheable
			if (!query.Iscacheable())
				return DecoratedRepository.Retrieve(context, query);

			// create the cache key for this node
			var cacheKey = query.CalculateCacheKey("Record_Set_Query_");

			// return the node
			return cachingService.GetOrAdd(context, cacheKey, () => DecoratedRepository.Retrieve(context, query).AsCacheableObject());
		}
		/// <summary>
		/// Creates a new record with the given <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties from which to create a record.</param>
		/// <returns>Returns the created <see cref="Record"/>.</returns>
		protected override Record DoCreate(IMansionContext context, IPropertyBag properties)
		{
			// execute in the decorated repository
			var record = DecoratedRepository.Create(context, properties);

			// clear the cache for the given record
			record.ClearFromCache(cachingService);

			return record;
		}
		/// <summary>
		/// Updates an existing <paramref name="record"/> in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be updated.</param>
		/// <param name="properties">The updated properties.</param>
		protected override void DoUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// execute in the decorated repository
			DecoratedRepository.Update(context, record, properties);

			// clear the cache for the given record
			record.ClearFromCache(cachingService);
		}
		/// <summary>
		/// Deletes an existing <paramref name="record"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be deleted.</param>
		protected override void DoDelete(IMansionContext context, Record record)
		{
			// execute in the decorated repository
			DecoratedRepository.Delete(context, record);

			// clear the cache for the given record
			record.ClearFromCache(cachingService);
		}
		#endregion
		#region Private Fields
		private readonly ICachingService cachingService;
		#endregion
	}
}