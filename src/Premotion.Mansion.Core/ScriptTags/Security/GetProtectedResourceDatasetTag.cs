using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Core.Security.Descriptors;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.ScriptTags.Security
{
	/// <summary>
	/// Gets a <see cref="Dataset"/> containing all the <see cref="ProtectedResource"/>s. 
	/// </summary>
	[Named(Constants.NamespaceUri, "getProtectedResourceDataset")]
	public class GetProtectedResourceDatasetTag : GetDatasetBaseTag
	{
		#region Overrides of GetDatasetBaseTag
		/// <summary>
		/// Gets the dataset.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(MansionContext context, IPropertyBag attributes)
		{
			// get the type service
			var typeService = context.Nucleus.Get<ITypeService>(context);

			// create the dataset
			var dataset = new Dataset();

			// loop over all the types
			foreach (var type in typeService.LoadAll(context))
			{
				// if the type does not have a security descriptor ignore it
				SecurityDescriptor securityDescriptor;
				if (!type.TryGetDescriptor(out securityDescriptor))
					continue;

				// get the resource model
				var resource = securityDescriptor.GetResource(context);

				// add the resource to the dataset
				dataset.AddRow(PropertyBagAdapterFactory.Adapt(context, resource));
			}

			// return the set
			return dataset;
		}
		#endregion
	}
}