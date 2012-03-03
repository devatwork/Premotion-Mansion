using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.ScriptTags.Stack;

namespace Premotion.Mansion.Core.ScriptTags.IO
{
	/// <summary>
	/// Enumerates the content of a directory.
	/// </summary>
	[Named(Constants.NamespaceUri, "enumerateApplicationFolder")]
	public class EnumerateApplicationFolderTag : GetDatasetBaseTag
	{
		/// <summary>
		/// Gets the dataset.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(MansionContext context, IPropertyBag attributes)
		{
			var dataset = new Dataset();
			var applicationResourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			foreach (var name in applicationResourceService.EnumeratorFolders(context, GetAttribute<string>(context, "path")))
			{
				dataset.AddRow(new PropertyBag
				               {
				               	{"name", name}
				               });
			}
			return dataset;
		}
	}
}