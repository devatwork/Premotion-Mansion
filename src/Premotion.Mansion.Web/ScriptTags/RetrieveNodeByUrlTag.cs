using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Retrieves a node based on the URL of the request.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveNodeByUrl")]
	public class RetrieveNodeByUrlTag : RetrieveNodeBaseTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nodeUrlService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RetrieveNodeByUrlTag(INodeUrlService nodeUrlService)
		{
			// validate arguments
			if (nodeUrlService == null)
				throw new ArgumentNullException("nodeUrlService");

			// set values
			this.nodeUrlService = nodeUrlService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override Node Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository)
		{
			// get the url
			Uri url;
			if (!arguments.TryGet(context, "url", out url))
				url = context.Cast<IMansionWebContext>().HttpContext.Request.Url;

			// parse the URL for identifiers
			IPropertyBag queryAttributes;
			if (!nodeUrlService.TryExtractQueryParameters(context.Cast<IMansionWebContext>(), url, out queryAttributes))
				return null;

			// parse the query
			var query = repository.ParseQuery(context, queryAttributes);

			// execute the query
			return repository.RetrieveSingle(context, query);
		}
		#endregion
		#region Private Fields
		private readonly INodeUrlService nodeUrlService;
		#endregion
	}
}