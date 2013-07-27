using System;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a parent node from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveParentNode")]
	public class RetrieveParentNodeTag : RetrieveRecordBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		public RetrieveParentNodeTag(IQueryParser parser) : base(parser)
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
		protected override Record Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository, IQueryParser parser)
		{
			// parse the query
			var query = parser.Parse(context, arguments);

			// make sure a child of clause is specified
			var parentOfSpecifications = query.Components.OfType<SpecificationQueryComponent>().Select(component => component.Specification).OfType<ParentOfSpecification>().ToArray();
			if (!parentOfSpecifications.Any())
				throw new InvalidOperationException("The child node was not specified.");

			// if there is a parent of specification for depth 0 it means a parent of the root node is queried, which does not exist
			if (parentOfSpecifications.Any(candidate => candidate.ChildPointer.Depth == 1))
				return null;

			// execute the query
			return repository.RetrieveSingleNode(context, query);
		}
		#endregion
	}
}