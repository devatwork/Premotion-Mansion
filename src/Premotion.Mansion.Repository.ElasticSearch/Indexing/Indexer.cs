using System;
using System.Net;
using Premotion.Mansion.Core;
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
		/// TODO
		/// </summary>
		/// <param name="context"></param>
		public void CreateIndices(IMansionContext context)
		{
			// validate arugments
			if (context == null)
				throw new ArgumentNullException("context");

			// resolve all the index definitions
			var definitions = indexDefinitionResolver.ResolveAll(context);

			// create each index
			foreach (var definition in definitions)
				CreateIndex(context, definition);
		}
		/// <summary>
		/// Creates the given index <paramref name="definition"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="definition">The <see cref="IndexDefinition"/>.</param>
		private void CreateIndex(IMansionContext context, IndexDefinition definition)
		{
			// check if the index exists
			if (connectionManager.Head(definition.Name, new[] {HttpStatusCode.OK, HttpStatusCode.NotFound}).StatusCode == HttpStatusCode.OK)
				connectionManager.Delete(definition.Name);

			connectionManager.Put(context, definition.Name, definition);
		}
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="context"></param>
		public void Reindex(IMansionContext context)
		{
			// validate arugments
			if (context == null)
				throw new ArgumentNullException("context");

			throw new NotImplementedException();
		}
		#endregion
		#region Private Fields
		private readonly ConnectionManager connectionManager;
		private readonly IndexDefinitionResolver indexDefinitionResolver;
		#endregion
	}
}