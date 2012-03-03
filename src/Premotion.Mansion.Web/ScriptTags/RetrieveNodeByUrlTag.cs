using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Retrieves a node based on the URL of the request.
	/// </summary>
	[Named(Constants.NamespaceUri, "retrieveNodeByUrl")]
	public class RetrieveNodeByUrlTag : RetrieveNodeBaseTag
	{
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override Node Retrieve(MansionContext context, IPropertyBag arguments, IRepository repository)
		{
			// get the url
			Uri url;
			if (!arguments.TryGet(context, "url", out url))
				url = context.Cast<MansionWebContext>().HttpContext.Request.Url;

			// parse the URL for identifiers
			IPropertyBag queryAttributes;
			if (!context.Nucleus.Get<INodeUrlService>(context).TryExtractQueryParameters(context, url, out queryAttributes))
				return null;

			// parse the query
			var query = repository.ParseQuery(context, queryAttributes);

			// execute the query
			return repository.RetrieveSingle(context, query);
		}
	}
}