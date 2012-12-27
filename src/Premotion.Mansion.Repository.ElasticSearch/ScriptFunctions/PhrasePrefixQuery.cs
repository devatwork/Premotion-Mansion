using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Specifications;

namespace Premotion.Mansion.Repository.ElasticSearch.ScriptFunctions
{
	/// <summary>
	/// Creates a phrase prefix specification.
	/// </summary>
	[ScriptFunction("PhrasePrefixQuery")]
	public class PhrasePrefixQuery : FunctionExpression
	{
		#region Evaluate Methods
		/// <summary>
		/// Creates a <see cref="PhrasePrefixQuerySpecification"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The query.</param>
		/// <param name="field">The name of the field on which to search.</param>
		/// <returns>Returns the <see cref="PhrasePrefixQuerySpecification"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public PhrasePrefixQuerySpecification Evaluate(IMansionContext context, string query, string field)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");
			if (string.IsNullOrEmpty(field))
				throw new ArgumentNullException("field");

			// construct the specification
			return new PhrasePrefixQuerySpecification(query, field);
		}
		#endregion
	}
}