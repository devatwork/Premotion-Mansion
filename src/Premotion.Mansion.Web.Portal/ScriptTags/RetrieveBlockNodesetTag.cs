using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Retrieves the block nodes for a node.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "retrieveBlockNodeset")]
	public class RetrieveBlockNodesetTag : RetrieveRecordSetBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		public RetrieveBlockNodesetTag(IQueryParser parser) : base(parser)
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
			// get the node
			var sourceNode = GetRequiredAttribute<Node>(context, "source");

			// retrieve the block nodes
			return repository.RetrieveNodeset(context, new PropertyBag
			                                           {
			                                           	{"baseType", "Block"},
			                                           	{"parentSource", sourceNode},
			                                           	{"status", NodeStatus.Published}
			                                           });
		}
		#endregion
	}
}