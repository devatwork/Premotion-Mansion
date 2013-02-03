using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.ScriptTags.Stack;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Provides base functionality for building nodes queries
	/// </summary>
	public abstract class RetrieveRecordBaseTag : GetRowBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		protected RetrieveRecordBaseTag(IQueryParser parser)
		{
			// validate arguments
			if (parser == null)
				throw new ArgumentNullException("parser");

			// set value
			this.parser = parser;
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Gets the row.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override IPropertyBag Get(IMansionContext context, IPropertyBag attributes)
		{
			// check if query source is specified
			IPropertyBag queryProperties;
			if (attributes.TryGetAndRemove(context, "querySource", out queryProperties))
				attributes = queryProperties.Copy().Merge(attributes);

			// invoke template method
			return Retrieve(context, attributes, context.Repository, parser);
		}
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository">The <see cref="IRepository"/>.</param>
		/// <param name="parser">The <see cref="IQueryParser"/>.</param>
		/// <returns>Returns the result.</returns>
		protected abstract Record Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository, IQueryParser parser);
		#endregion
		#region Private Fields
		private readonly IQueryParser parser;
		#endregion
	}
}