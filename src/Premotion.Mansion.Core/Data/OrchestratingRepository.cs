using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Prioritized;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Orchestractes all the <see cref="BaseStorageEngine"/>s, <see cref="BaseIndexEngine"/>s and <see cref="BaseQueryEngine"/>s.
	/// </summary>
	public class OrchestratingRepository : RepositoryBase
	{
		#region Constructors
		/// <summary>
		/// Constructs this orchestrating repository with the given <paramref name="storageEngine"/>, <see cref="queryEngines"/> and <paramref name="indexEngines"/>.
		/// </summary>
		/// <param name="storageEngine">The <see cref="BaseStorageEngine"/> which stores all the data.</param>
		/// <param name="queryEngines">The <see cref="BaseQueryEngine"/>s which can retrieve the data.</param>
		/// <param name="indexEngines">The <see cref="BaseIndexEngine"/>s which can index the data.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public OrchestratingRepository(BaseStorageEngine storageEngine, IEnumerable<BaseQueryEngine> queryEngines, IEnumerable<BaseIndexEngine> indexEngines)
		{
			// validate arguments
			if (storageEngine == null)
				throw new ArgumentNullException("storageEngine");
			if (queryEngines == null)
				throw new ArgumentNullException("queryEngines");
			if (indexEngines == null)
				throw new ArgumentNullException("indexEngines");

			// set the engines
			this.storageEngine = storageEngine;
			this.queryEngines = queryEngines.ToArray();
			this.indexEngines = indexEngines.ToArray();
		}
		#endregion
		#region Overrides of RepositoryBase
		/// <summary>
		/// Starts this object. This methods must be called after the object has been created and before it is used.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoStart(IMansionContext context)
		{
			// nothing to do here
		}
		#endregion
		#region Record Methods
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Record"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override Record DoRetrieveSingle(IMansionContext context, Query query)
		{
			// select the engine
			var searcher = SelectQueryEngine(context, query);

			// execute the query
			return searcher.RetrieveSingle(context, query);
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
			// select the engine
			var searcher = SelectQueryEngine(context, query);

			// execute the query
			return searcher.Retrieve(context, query);
		}
		/// <summary>
		/// Creates a new record with the given <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties from which to create a record.</param>
		/// <returns>Returns the created <see cref="Record"/>.</returns>
		protected override Record DoCreate(IMansionContext context, IPropertyBag properties)
		{
			// store the record
			var record = storageEngine.Create(context, properties);

			// index the record
			foreach (var indexer in indexEngines)
				indexer.Index(context, record);

			// return the record
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
			// store the record
			var updated = storageEngine.Update(context, record, properties);

			// index the record
			foreach (var indexer in indexEngines)
				indexer.Index(context, updated);
		}
		/// <summary>
		/// Deletes an existing <paramref name="record"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be deleted.</param>
		protected override void DoDelete(IMansionContext context, Record record)
		{
			// remove from indexes
			foreach (var indexer in indexEngines)
				indexer.Delete(context, record);

			// delete the record
			storageEngine.Delete(context, record);
		}
		#endregion
		#region Node Methods
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns the node.</returns>
		protected override Node DoRetrieveSingleNode(IMansionContext context, Query query)
		{
			// select the engine
			var searcher = SelectQueryEngine(context, query);

			// execute the query
			return searcher.RetrieveSingleNode(context, query);
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns></returns>
		protected override Nodeset DoRetrieveNodeset(IMansionContext context, Query query)
		{
			// select the engine
			var searcher = SelectQueryEngine(context, query);

			// execute the query
			return searcher.RetrieveNodeset(context, query);
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
			// store the node
			var record = storageEngine.CreateNode(context, parent, newProperties);

			// index the record
			foreach (var indexer in indexEngines)
				indexer.Index(context, record);

			// return the record
			return record;
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected override void DoUpdateNode(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// store the record
			var updated = storageEngine.UpdateNode(context, node, modifiedProperties);

			// index the record
			foreach (var indexer in indexEngines)
				indexer.Index(context, updated);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The pointer to the node which will be deleted.</param>
		protected override void DoDeleteNode(IMansionContext context, Node node)
		{
			// remove from indexes
			foreach (var indexer in indexEngines)
				indexer.Delete(context, node);

			// delete the record
			storageEngine.Delete(context, node);
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
			// move the node
			var record = storageEngine.MoveNode(context, pointer, newParentPointer);

			// index the record
			foreach (var indexer in indexEngines)
				indexer.Index(context, record);

			// return the record
			return record;
		}
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		protected override Node DoCopyNode(IMansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			// copy the node
			var record = storageEngine.CopyNode(context, pointer, newParentPointer);

			// index the record
			foreach (var indexer in indexEngines)
				indexer.Index(context, record);

			// return the record
			return record;
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Select the best <see cref="BaseQueryEngine"/> to execute the <paramref name="query"/> on.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which needs to be executed.</param>
		/// <returns>Returns the <see cref="BaseQueryEngine"/> which to use to execute <paramref name="query"/>.</returns>
		private BaseQueryEngine SelectQueryEngine(IMansionContext context, Query query)
		{
			return queryEngines.Elect(context, query, engines => engines.OrderByPriority().First());
		}
		#endregion
		#region Private Fields
		private readonly BaseIndexEngine[] indexEngines;
		private readonly BaseQueryEngine[] queryEngines;
		private readonly BaseStorageEngine storageEngine;
		#endregion
	}
}