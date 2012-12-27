using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Repository.ElasticSearch.Querying;

namespace Premotion.Mansion.Repository.ElasticSearch.ScriptTags
{
	/// <summary>
	/// Performans an ElasticSearch search.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "search")]
	public class SearchTag : RetrieveRecordSetBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="searcher"></param>
		/// <param name="parser"> </param>
		public SearchTag(Searcher searcher, IQueryParser parser) : base(parser)
		{
			// validate arguments
			if (searcher == null)
				throw new ArgumentNullException("searcher");

			// set values
			this.searcher = searcher;
		}
		#endregion
		#region Overrides of RetrieveRecordSetBaseTag
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository">The <see cref="IRepository"/>.</param>
		/// <param name="parser">The <see cref="IQueryParser"/>.</param>
		/// <returns>Returns the <see cref="RecordSet"/>.</returns>
		protected override RecordSet Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository, IQueryParser parser)
		{
			// parse them into a query
			var query = parser.Parse(context, arguments);

			// execute the query
			return searcher.Search(context, query);
		}
		#endregion
		#region Private Fields
		private readonly Searcher searcher;
		#endregion
	}
}