using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.ScriptTags
{
	/// <summary>
	/// Renders the specified block.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "renderBlock")]
	public class RenderBlockTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			var blockProperties = GetAttributes(context);
			string targetField;
			if (!blockProperties.TryGetAndRemove(context, "targetField", out targetField) || string.IsNullOrEmpty(targetField))
				throw new AttributeNullException("targetField", this);

			var portalService = context.Nucleus.ResolveSingle<IPortalService>();
			portalService.RenderBlockToOutput(context, blockProperties, targetField);
		}
		#endregion
	}
}