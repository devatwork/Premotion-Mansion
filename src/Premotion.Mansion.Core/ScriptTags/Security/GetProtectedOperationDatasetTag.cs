using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptTags.Security
{
	/// <summary>
	/// Gets a <see cref="Dataset"/> containing all the <see cref="ProtectedOperation"/>s. 
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "getProtectedOperationDataset")]
	public class GetProtectedOperationDatasetTag : GetDatasetBaseTag
	{
		#region Overrides of GetDatasetBaseTag
		/// <summary>
		/// Gets the dataset.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(IMansionContext context, IPropertyBag attributes)
		{
			// get the resource properties
			var resource = GetRequiredAttribute<ProtectedResource>(context, "source");

			// create the dataset
			var dataset = new Dataset();

			// loop over all the operations
			foreach (var operation in resource.Operations)
			{
				// add the resource to the dataset
				dataset.AddRow(PropertyBagAdapterFactory.Adapt(context, operation));
			}

			// return the set
			return dataset;
		}
		#endregion
	}
}