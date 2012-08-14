using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.ScriptTags.Repository;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Retrieves a site node byt it's host name.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveSiteNode")]
	public class RetrieveSiteNodeTag : RetrieveRecordBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		public RetrieveSiteNodeTag(IQueryParser parser) : base(parser)
		{
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
		protected override IPropertyBag Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository, IQueryParser parser)
		{
			// get the url
			Uri url;
			if (!arguments.TryGet(context, "url", out url))
				url = context.Cast<IMansionWebContext>().HttpContext.Request.Url;

			// parse the query
			var query = parser.Parse(context, new PropertyBag
			                                  {
			                                  	{"baseType", "Site"},
			                                  	{"hostHeaders", url.DnsSafeHost}
			                                  });

			// execute the query
			return repository.RetrieveSingleNode(context, query);
		}
		#endregion
	}
}