using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Fetches a node from the stack and puts it into a new dataspace.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "fetchNode")]
	public class FetchNodeTag : RetrieveRecordBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		public FetchNodeTag(IQueryParser parser) : base(parser)
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
			return GetAttribute<Node>(context, "source");
		}
		#endregion
	}
}