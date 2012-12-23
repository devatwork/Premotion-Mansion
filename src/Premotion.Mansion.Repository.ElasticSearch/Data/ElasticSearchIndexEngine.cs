using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.ElasticSearch.Indexing;

namespace Premotion.Mansion.Repository.ElasticSearch.Data
{
	/// <summary>
	/// Implements the <see cref="BaseIndexEngine"/> for ElasticSearch.
	/// </summary>
	public class ElasticSearchIndexEngine : BaseIndexEngine
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="ElasticSearchIndexEngine"/>.
		/// </summary>
		/// <param name="indexer">The <see cref="Indexer"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public ElasticSearchIndexEngine(Indexer indexer)
		{
			// validate arguments
			if (indexer == null)
				throw new ArgumentNullException("indexer");

			// set the values
			this.indexer = indexer;
		}
		#endregion
		#region Overrides of BaseIndexEngine
		/// <summary>
		/// Indexes the given <paramref name="record"/> into this engine.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to index.</param>
		protected override void DoIndex(IMansionContext context, Record record)
		{
			indexer.Index(context, record);
		}
		/// <summary>
		/// Deletes the given <paramref name="record"/> from this engine.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to delete.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="record"/> is null.</exception>
		protected override void DoDelete(IMansionContext context, Record record)
		{
			indexer.Delete(context, record);
		}
		#endregion
		#region Private Fields
		private readonly Indexer indexer;
		#endregion
	}
}