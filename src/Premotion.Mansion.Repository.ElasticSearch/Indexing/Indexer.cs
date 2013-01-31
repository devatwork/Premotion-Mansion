using System;
using System.Net;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Retry;
using Premotion.Mansion.Repository.ElasticSearch.Connection;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

namespace Premotion.Mansion.Repository.ElasticSearch.Indexing
{
	/// <summary>
	/// Implements the ElasticSearch indexer service.
	/// </summary>
	public class Indexer
	{
		#region Constructors
		/// <summary>
		/// Constructs the index service.
		/// </summary>
		/// <param name="connectionManager">The <see cref="connectionManager"/>.</param>
		/// <param name="indexDefinitionResolver">The <see cref="IndexDefinitionResolver"/>.</param>
		public Indexer(ConnectionManager connectionManager, IndexDefinitionResolver indexDefinitionResolver)
		{
			// validate arguments
			if (connectionManager == null)
				throw new ArgumentNullException("connectionManager");
			if (indexDefinitionResolver == null)
				throw new ArgumentNullException("indexDefinitionResolver");

			// set values
			this.connectionManager = connectionManager;
			this.indexDefinitionResolver = indexDefinitionResolver;
		}
		#endregion
		#region Index Methods
		/// <summary>
		/// Creates all the indices in this application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void CreateIndices(IMansionContext context)
		{
			// validate arugments
			if (context == null)
				throw new ArgumentNullException("context");

			// resolve all the index definitions
			var definitions = indexDefinitionResolver.ResolveAll(context);

			// create each index
			foreach (var definition in definitions)
				CreateIndex(definition);
		}
		/// <summary>
		/// Creates the given index <paramref name="definition"/>.
		/// </summary>
		/// <param name="definition">The <see cref="IndexDefinition"/>.</param>
		private void CreateIndex(IndexDefinition definition)
		{
			new Action(() => {
				// delete the index if it exists
				if (connectionManager.Head(definition.Name, new[] {HttpStatusCode.OK, HttpStatusCode.NotFound}).StatusCode == HttpStatusCode.OK)
					connectionManager.Delete(definition.Name);

				// create the index
				connectionManager.Put(definition.Name, definition);
			}).Retry(new FixedIntervalStrategy(3, TimeSpan.FromMilliseconds(100)));
		}
		/// <summary>
		/// Reindexes all the content in the top-most repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void Reindex(IMansionContext context)
		{
			// validate arugments
			if (context == null)
				throw new ArgumentNullException("context");

			// retrieve all the data directly from the repository
			var recordSet = context.Repository.RetrieveNodeset(context, new PropertyBag {
				{"cache", false},
				{"baseType", "Default"},
				{"bypassAuthorization", true},
				{StorageOnlyQueryComponent.PropertyKey, true}
			});

			// index the dataset
			Index(context, recordSet);
		}
		/// <summary>
		/// Indexes the given <paramref name="recordSet"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="recordSet">The <see cref="RecordSet"/>.</param>
		public void Index(IMansionContext context, RecordSet recordSet)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (recordSet == null)
				throw new ArgumentNullException("recordSet");

			// loop over all the records and index them
			// TODO: optimize using batch processesing?
			foreach (var record in recordSet.Records)
				Index(context, record);
		}
		/// <summary>
		/// Indexes the given <paramref name="record"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/>.</param>
		public void Index(IMansionContext context, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");

			// resolve the index definitions of this record
			var indexDefinitions = indexDefinitionResolver.Resolve(context, record.Type);

			// loop over all the definition
			// TODO: implement batching
			foreach (var indexDefinition in indexDefinitions)
			{
				// find the mapper for this record
				var mapping = indexDefinition.FindTypeMapping(record.Type);

				// transform the record into a document
				var document = mapping.Transform(context, record);

				// determine the resource
				var resource = indexDefinition.Name + '/' + mapping.Name + '/' + record.Id;

				// index the document
				new Action(() => connectionManager.Put(resource, document, new[] {HttpStatusCode.Created, HttpStatusCode.OK})).Retry(new FixedIntervalStrategy(3, TimeSpan.FromMilliseconds(100)));
			}
		}
		#endregion
		#region Delete Methods
		/// <summary>
		/// Deletes the given <paramref name="record"/> from the index.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="record"/> is null.</exception>
		public void Delete(IMansionContext context, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");

			// TODO: implement delete children

			// resolve the index definitions of this record
			var indexDefinitions = indexDefinitionResolver.Resolve(context, record.Type);

			// loop over all the definition
			foreach (var indexDefinition in indexDefinitions)
			{
				// find the mapper for this record
				var mapping = indexDefinition.FindTypeMapping(record.Type);

				// determine the resource
				var resource = indexDefinition.Name + '/' + mapping.Name + '/' + record.Id;

				// index the document
				new Action(() => connectionManager.Delete(resource, new[] {HttpStatusCode.OK, HttpStatusCode.NotFound})).Retry(new FixedIntervalStrategy(3, TimeSpan.FromMilliseconds(100)));
			}
		}
		#endregion
		#region Optimize Methods
		/// <summary>
		/// Optimizes all the indices within the given ElasticSearch instance.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void OptimizeAll(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// resolve all the index definitions
			var definitions = indexDefinitionResolver.ResolveAll(context);

			// create each index
			foreach (var definition in definitions)
				Optimize(context, definition);
		}
		/// <summary>
		/// Optimizes the indexes as defined by <paramref name="definition"/> within the given ElasticSearch instance.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="definition">The <see cref="IndexDefinition"/> which to optimize.</param>
		public void Optimize(IMansionContext context, IndexDefinition definition)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (definition == null)
				throw new ArgumentNullException("definition");

			// format the resource
			var resource = definition.Name + "/_optimize";

			// execute the request
			new Action(() => connectionManager.Post(resource)).Retry(new FixedIntervalStrategy(3, TimeSpan.FromMilliseconds(100)));
		}
		#endregion
		#region Private Fields
		private readonly ConnectionManager connectionManager;
		private readonly IndexDefinitionResolver indexDefinitionResolver;
		#endregion
	}
}