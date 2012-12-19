using System;
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

			// loop over all

			throw new NotImplementedException();
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