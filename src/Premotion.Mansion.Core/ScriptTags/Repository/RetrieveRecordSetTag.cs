using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a <see cref="Dataset"/> from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveRecordSet")]
	public class RetrieveRecordSetTag : RetrieveRecordSetBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		public RetrieveRecordSetTag(IQueryParser parser) : base(parser)
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

			// execute and return the result
			return repository.Retrieve(context, query);
		}
		#endregion
	}
}