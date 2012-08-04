﻿using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves child nodes from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveChildNodeset")]
	public class RetrieveChildNodesetTag : RetrieveDatasetBaseTag
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

			// make sure a child of clause is specified
			if (!query.HasClause<ChildOfClause>())
				throw new InvalidOperationException("The parent node was not specified.");

			// execute the query
			return repository.Retrieve(context, query);
		}
	}
}