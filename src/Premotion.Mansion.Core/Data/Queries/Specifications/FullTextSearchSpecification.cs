﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries.Specifications
{
	/// <summary>
	/// Specifies the full-text search query.
	/// </summary>
	public class FullTextSearchSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs the full-text search specification using the given <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The full-text query.</param>
		/// <exception cref="ArgumentNullException">Thrown</exception>
		public FullTextSearchSpecification(string query)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");

			// set values
			Query = query;
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Gets the names of the properties used by this query component.
		/// </summary>
		/// <returns>Returns the property hints.</returns>
		protected override IEnumerable<string> DoGetPropertyHints()
		{
			return new[] {"name", "description", "body", "fullText"};
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append("full-text-search:").Append(Query);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the full-text query.
		/// </summary>
		public string Query { get; private set; }
		#endregion
	}
}