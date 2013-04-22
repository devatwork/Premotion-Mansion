using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Linking.ScriptTags
{
	/// <summary>
	/// Gets all the incoming links of a given source identified by their link name.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "getIncomingLinksDataset")]
	public class GetIncomingLinksDatasetTag : GetDatasetBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="linkService"></param>
		public GetIncomingLinksDatasetTag(ILinkService linkService)
		{
			this.linkService = linkService;
		}
		#endregion
		#region Overrides of GetDatasetBaseTag
		/// <summary>
		/// Gets the dataset.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(IMansionContext context, IPropertyBag attributes)
		{
			// get the attribute values
			var source = GetRequiredAttribute<IPropertyBag>(context, "source");
			var linkName = GetRequiredAttribute<string>(context, "linkName");

			// get the linkbase
			var linkbase = linkService.GetLinkbase(context, source);

			// get the link definition by its name
			var linkDefinition = linkbase.Definition.GetLinkDefinition(linkName);

			// get the links
			var dataset = new Dataset();
			foreach (var link in linkbase.Links.Incoming().OfType(linkDefinition))
				dataset.AddRow(link.Properties);
			return dataset;
		}
		#endregion
		#region Private Fields
		private readonly ILinkService linkService;
		#endregion
	}
}