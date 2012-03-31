using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Service
{
	/// <summary>
	/// Provides portal related services.
	/// </summary>
	public interface IPortalService
	{
		#region Template Page Methods
		/// <summary>
		/// Resolves the template page for an particual content node..
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="siteNode">The site <see cref="Node"/>.</param>
		/// <param name="contentNode">The content <see cref="Node"/>.</param>
		/// <param name="templatePageNode">When found, the template page <see cref="Node"/>.</param>
		/// <returns>Returns true when the template page could be resolved, otherwise false.</returns>
		bool TryResolveTemplatePage(IMansionContext context, Node siteNode, Node contentNode, out Node templatePageNode);
		#endregion
		#region Column Methods
		/// <summary>
		/// Renders a column with the specified <paramref name="columnName"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="columnName">The name of the column which to render.</param>
		/// <param name="ownerProperties">The <see cref="IPropertyBag"/> to which the column belongs.</param>
		/// <param name="blockDataset">The <see cref="Dataset"/> containing the all blocks of the <paramref name="ownerProperties"/>.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		void RenderColumn(IMansionContext context, string columnName, IPropertyBag ownerProperties, Dataset blockDataset, string targetField);
		/// <summary>
		/// Gets a <see cref="Dataset"/> containing all the columns available for the specified <paramref name="type"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/> for which to get the column <see cref="Dataset"/>.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing all the columns.</returns>
		Dataset GetColumnDataset(IMansionContext context, ITypeDefinition type);
		#endregion
		#region Block Methods
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		void RenderBlock(IMansionContext context, IPropertyBag blockProperties, string targetField);
		#endregion
	}
}