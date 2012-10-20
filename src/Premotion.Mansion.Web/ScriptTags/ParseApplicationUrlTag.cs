using Premotion.Mansion.Core;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Parses an application url.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "parseApplicationUrl")]
	public class ParseApplicationUrlTag : GetRowBaseTag
	{
		#region Overrides of GetRowBaseTag
		/// <summary>
		/// Gets the row.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override IPropertyBag Get(IMansionContext context, IPropertyBag attributes)
		{
			// get the url
			Url url;
			if (!attributes.TryGet(context, "url", out url))
				url = context.Cast<IMansionWebContext>().Request.RequestUrl;

			// return the property bag representation
			return url.ToPropertyBag();
		}
		#endregion
	}
}