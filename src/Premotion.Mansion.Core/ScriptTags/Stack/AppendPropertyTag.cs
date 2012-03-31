using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Append a string property.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "appendProperty")]
	public class AppendPropertyTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the attributes
			var dataspace = GetRequiredAttribute<IPropertyBag>(context, "dataspace");
			var property = GetRequiredAttribute<string>(context, "property");

			// get the original property value
			var originalValue = dataspace.Get(context, property, string.Empty);

			// get the value which to append
			var valueToAppend = GetAttribute(context, "value", string.Empty);

			// determine the new value
			var separator = GetAttribute(context, "separator", ",");
			var newValue = string.Empty;
			if (!string.IsNullOrEmpty(originalValue) && !string.IsNullOrEmpty(valueToAppend))
				newValue = originalValue + separator + valueToAppend;
			else if (!string.IsNullOrEmpty(originalValue))
				newValue = originalValue;
			else if (!string.IsNullOrEmpty(valueToAppend))
				newValue = valueToAppend;

			// set the value
			dataspace.Set(property, newValue);
		}
	}
}