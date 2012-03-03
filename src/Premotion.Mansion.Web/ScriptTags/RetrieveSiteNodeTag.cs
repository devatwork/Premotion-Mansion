using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Repository;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Retrieves a site node byt it's host name.
	/// </summary>
	[Named(Constants.NamespaceUri, "retrieveSiteNode")]
	public class RetrieveSiteNodeTag : RetrieveNodeBaseTag
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

			// parse the query
			var query = repository.ParseQuery(context, new PropertyBag
			                                           {
			                                           	{"baseType", "Site"},
			                                           	{"hostHeaders", url.DnsSafeHost}
			                                           });

			// execute the query
			return repository.RetrieveSingle(context, query);
		}
	}
}