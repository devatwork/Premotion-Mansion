using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;

namespace Premotion.Mansion.Web.Cms.ScriptTags
{
	/// <summary>
	/// Gets a dataset of all the CMS plugins available.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "getPluginDataset")]
	public class GetPluginDatasetTag : GetDatasetBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public GetPluginDatasetTag(ITypeService typeService)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
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
			// create the dataset
			var dataset = new Dataset();

			// loop over all the CMS plugin types
			foreach (var plugin in typeService.Load(context, "CmsPlugin").GetInheritingTypes(context).Select(type =>
			                                                                                                 {
			                                                                                                 	// get the plugin descriptor
			                                                                                                 	CmsPluginDescriptor descriptor;
			                                                                                                 	if (!type.TryGetDescriptor(out descriptor))
			                                                                                                 		throw new InvalidOperationException(string.Format("Type '{0}' does not contain a CMS-plugin", type.Name));

			                                                                                                 	// return the model
			                                                                                                 	return new
			                                                                                                 	       {
			                                                                                                 	       	Type = type,
			                                                                                                 	       	Descriptor = descriptor
			                                                                                                 	       };
			                                                                                                 }).OrderBy(plugin => plugin.Descriptor.GetOrder(context)))
			{
				// create the plugin row
				var row = new PropertyBag
				          {
				          	{"type", plugin.Type.Name}
				          };

				// add the row to the dataset
				dataset.AddRow(row);
			}

			return dataset;
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}