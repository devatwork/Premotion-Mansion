using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a <see cref="IPropertyBag"/> from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveRecord")]
	public class RetrieveRecordTag : RetrieveRecordBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		public RetrieveRecordTag(IQueryParser parser) : base(parser)
		{
		}
		#endregion
		#region Overrides of RetrieveRecordBaseTag
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository">The <see cref="IRepository"/>.</param>
		/// <param name="parser">The <see cref="IQueryParser"/>.</param>
		/// <returns>Returns the result.</returns>
		protected override IPropertyBag Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository, IQueryParser parser)
		{
			// parse the query
			var query = parser.Parse(context, arguments);

			// execute and return the result
			return repository.RetrieveSingle(context, query);
		}
		#endregion
	}
}