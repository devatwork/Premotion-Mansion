using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Repository.ElasticSearch.Indexing;

namespace Premotion.Mansion.Repository.ElasticSearch.ScriptTags
{
	/// <summary>
	/// Syncs the nodes table to other tables.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "reindex")]
	public class ReindexTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="indexer"></param>
		public ReindexTag(Indexer indexer)
		{
			// validate arguments
			if (indexer == null)
				throw new ArgumentNullException("indexer");

			// set values
			this.indexer = indexer;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			indexer.CreateIndices(context);
			indexer.Reindex(context);
		}
		#endregion
		#region Private Fields
		private readonly Indexer indexer;
		#endregion
	}
}