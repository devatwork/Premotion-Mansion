using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Connection;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

namespace Premotion.Mansion.Repository.ElasticSearch.Quering
{
	/// <summary>
	/// Implements the ElasticSearch searcher interface.
	/// </summary>
	public class Searcher
	{
		#region Constructors
		/// <summary>
		/// Constructs the index service.
		/// </summary>
		/// <param name="connectionManager">The <see cref="connectionManager"/>.</param>
		/// <param name="indexDefinitionResolver">The <see cref="IndexDefinitionResolver"/>.</param>
		public Searcher(ConnectionManager connectionManager, IndexDefinitionResolver indexDefinitionResolver)
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
		#region Search Methods
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="context"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		public RecordSet Search(IMansionContext context, Query query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");

			throw new System.NotImplementedException();
		}
		#endregion
		#region Private Fields
		private readonly ConnectionManager connectionManager;
		private readonly IndexDefinitionResolver indexDefinitionResolver;
		#endregion
	}
}