using System;
using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Specifications
{
	/// <summary>
	/// Represens a phrase prefix specification.
	/// </summary>
	public class PhrasePrefixQuerySpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs the PhrasePrefixQuerySpecification.
		/// </summary>
		/// <param name="query">The input for the query.</param>
		/// <param name="field">The field name on which to search.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public PhrasePrefixQuerySpecification(string query, string field)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");
			if (string.IsNullOrEmpty(field))
				throw new ArgumentNullException("field");

			// set the values
			Query = query;
			Field = field;
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Gets the names of the properties used by this query component.
		/// </summary>
		/// <returns>Returns the property hints.</returns>
		protected override IEnumerable<string> DoGetPropertyHints()
		{
			return new[] {Field.GetFieldNameBase()};
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.AppendFormat("text_phrase_prefix={0}:{1}", Field, Query);
		}
		#endregion
		#region Properties
		/// <summary>
		/// The field name on which to search.
		/// </summary>
		public string Field { get; private set; }
		/// <summary>
		/// The input for the query.
		/// </summary>
		public string Query { get; private set; }
		#endregion
	}
}