using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a single node from the repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveNode")]
	public class RetrieveNodeTag : RetrieveRecordBaseTag
	{
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override IPropertyBag Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository)
		{
			// parse the query
			var query = repository.ParseQuery(context, arguments);

			// make sure a child of clause is specified
			if (!query.HasClause<IdClause>() && !query.HasClause<PermanentIdClause>())
				throw new InvalidOperationException("The (permanent) ID of the node was not specified");

			// execute the query
			return repository.RetrieveSingleNode(context, query);
		}
	}
}