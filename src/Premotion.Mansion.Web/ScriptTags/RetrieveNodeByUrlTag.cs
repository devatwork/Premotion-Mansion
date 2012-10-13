using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Urls;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Retrieves a node based on the URL of the request.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveNodeByUrl")]
	public class RetrieveNodeByUrlTag : RetrieveRecordBaseTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="nodeUrlService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RetrieveNodeByUrlTag(IQueryParser parser, INodeUrlService nodeUrlService) : base(parser)
		{
			// validate arguments
			if (nodeUrlService == null)
				throw new ArgumentNullException("nodeUrlService");

			// set values
			this.nodeUrlService = nodeUrlService;
		}
		#endregion
		#region Overrides of RetrieveRecordBaseTag
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository">The <see cref="IRepository"/>.</param>
		/// <param name="parser">The <see cref="IQueryParser"/>.</param>
		/// <returns>Returns the result.</returns>
		protected override Record Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository, IQueryParser parser)
		{
			// get the url
			Url url;
			if (!arguments.TryGet(context, "url", out url))
				url = context.Cast<IMansionWebContext>().Request.Url;

			// parse the URL for identifiers
			IPropertyBag queryAttributes;
			if (!nodeUrlService.TryExtractQueryParameters(context.Cast<IMansionWebContext>(), url, out queryAttributes))
				return null;

			// parse the query
			var query = parser.Parse(context, queryAttributes);

			// execute the query
			return repository.RetrieveSingleNode(context, query);
		}
		#endregion
		#region Private Fields
		private readonly INodeUrlService nodeUrlService;
		#endregion
	}
}