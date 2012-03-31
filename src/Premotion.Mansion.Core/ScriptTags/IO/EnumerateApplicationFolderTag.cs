using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.IO
{
	/// <summary>
	/// Enumerates the content of a directory.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "enumerateApplicationFolder")]
	public class EnumerateApplicationFolderTag : GetDatasetBaseTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public EnumerateApplicationFolderTag(IApplicationResourceService applicationResourceService)
		{
			// validate arguments
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");

			// set values
			this.applicationResourceService = applicationResourceService;
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
			var dataset = new Dataset();
			foreach (var name in applicationResourceService.EnumeratorFolders(context, GetAttribute<string>(context, "path")))
			{
				dataset.AddRow(new PropertyBag
				               {
				               	{"name", name}
				               });
			}
			return dataset;
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		#endregion
	}
}