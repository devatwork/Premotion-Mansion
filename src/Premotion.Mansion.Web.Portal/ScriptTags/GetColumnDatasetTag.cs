using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Gets a <see cref="Dataset"/> containing all the columns available for the specified type.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "getColumnDataset")]
	public class GetColumnDatasetTag : GetDatasetBaseTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public GetColumnDatasetTag(IPortalService portalService)
		{
			// validate arguments
			if (portalService == null)
				throw new ArgumentNullException("portalService");

			// set values
			this.portalService = portalService;
		}
		#endregion
		#region Overrides of GetDatasetBaseTag
		/// <summary>
		/// Gets a <see cref="Dataset"/> containing all the columns available for the specified type.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(IMansionContext context, IPropertyBag attributes)
		{
			return portalService.GetColumnDataset(context, attributes.Get<ITypeDefinition>(context, "type"));
		}
		#endregion
		#region Private Fields
		private readonly IPortalService portalService;
		#endregion
	}
}