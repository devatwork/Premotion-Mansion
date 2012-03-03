using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Gets a <see cref="Dataset"/> containing all the columns available for the specified type.
	/// </summary>
	[Named(Constants.TagNamespaceUri, "getColumnDataset")]
	public class GetColumnDatasetTag : GetDatasetBaseTag
	{
		#region Overrides of GetDatasetBaseTag
		/// <summary>
		/// Gets a <see cref="Dataset"/> containing all the columns available for the specified type.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(MansionContext context, IPropertyBag attributes)
		{
			// get the portal service
			var portalService = context.Nucleus.Get<IPortalService>(context);

			// get the dataset
			return portalService.GetColumnDataset(context, attributes.Get<ITypeDefinition>(context, "type"));
		}
		#endregion
	}
}