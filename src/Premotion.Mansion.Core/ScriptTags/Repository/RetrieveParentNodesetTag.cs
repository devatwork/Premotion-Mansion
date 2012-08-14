using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves parent nodes from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveParentNodeset")]
	public class RetrieveParentNodesetTag : RetrieveDatasetBaseTag
	{
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository)
		{
			// parse the query
			var query = repository.ParseQuery(context, arguments);

			// make sure a parent of clause is specified
			if (!query.HasClause<ParentOfClause>())
				throw new InvalidOperationException("The child node was not specified.");

			// execute the query
			return repository.RetrieveNodeset(context, query);
		}
	}
}