using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Copies properties from source dataspace to that target dataspace.
	/// </summary>
	[Named(Constants.NamespaceUri, "copyProperties")]
	public class CopyPropertiesTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get arguments
			var sourceDataspace = GetRequiredAttribute<IPropertyBag>(context, "source");
			var targetDataspace = GetRequiredAttribute<IPropertyBag>(context, "target");

			// copy
			foreach (var property in sourceDataspace)
				targetDataspace.Set(property.Key, property.Value);
		}
	}
}