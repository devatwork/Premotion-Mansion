using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves child nodes from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveChildNodeset")]
	public class RetrieveChildNodesetTag : RetrieveRecordSetBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		public RetrieveChildNodesetTag(IQueryParser parser) : base(parser)
		{
		}
		#endregion
		#region Overrides of RetrieveDatasetBaseTag
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
			// parse the query
			var query = parser.Parse(context, arguments);

			// make sure a child of clause is specified
			if (!query.HasSpecification<ChildOfSpecification>())
				throw new InvalidOperationException("The parent node was not specified.");

			// execute the query
			return repository.RetrieveNodeset(context, query);
		}
		#endregion
	}
}