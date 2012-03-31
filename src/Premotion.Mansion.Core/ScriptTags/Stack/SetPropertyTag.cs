using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Opens a template.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "setProperty")]
	public class SetPropertyTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the attributes
			var dataspace = GetRequiredAttribute<IPropertyBag>(context, "dataspace");
			var property = GetRequiredAttribute<string>(context, "property");

			// set the value
			dataspace.Set(property, GetAttribute<object>(context, "value"));
		}
	}
}