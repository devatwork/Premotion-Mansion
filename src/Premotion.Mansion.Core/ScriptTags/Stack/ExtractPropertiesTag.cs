using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Extracts properties from source dataspace and store them in the target dataspace.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "extractProperties")]
	public class ExtractPropertiesTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get arguments
			var sourceDataspace = GetRequiredAttribute<IPropertyBag>(context, "source");
			var targetDataspace = GetRequiredAttribute<IPropertyBag>(context, "target");
			var whitelist = (GetAttribute(context, "whitelist", string.Join(",", sourceDataspace.Names)) ?? string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();

			// copy all the properties on the whitelist
			foreach (var propertyName in whitelist)
			{
				object value;
				if (sourceDataspace.TryGetAndRemove(context, propertyName, out value))
					targetDataspace.Set(propertyName, value);
			}
		}
	}
}