using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Repository.ElasticSearch.Connection;

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
		public Indexer(ConnectionManager connectionManager)
		{
			// validate arguments
			if (connectionManager == null)
				throw new ArgumentNullException("connectionManager");

			// set values
			this.connectionManager = connectionManager;
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
		#endregion
	}
}