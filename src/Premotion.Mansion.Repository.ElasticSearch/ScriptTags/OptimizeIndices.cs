using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Repository.ElasticSearch.Indexing;

namespace Premotion.Mansion.Repository.ElasticSearch.ScriptTags
{
	/// <summary>
	/// Optimizes all the indices within the given ElasticSearch instance.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "optimizeIndices")]
	public class OptimizeIndices : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="indexer"></param>
		public OptimizeIndices(Indexer indexer)
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
			indexer.OptimizeAll(context);
		}
		#endregion
		#region Private Fields
		private readonly Indexer indexer;
		#endregion
	}
}