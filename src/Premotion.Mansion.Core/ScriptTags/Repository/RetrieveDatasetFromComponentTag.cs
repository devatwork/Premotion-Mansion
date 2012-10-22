using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a <see cref="RecordSet"/> from a given component.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveDatasetFromComponent")]
	public class RetrieveDatasetFromComponentTag : GetDatasetBaseTag
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
			// get the component and method names
			string componentName;
			if (!attributes.TryGetAndRemove(context, "componentName", out componentName))
				throw new AttributeNullException("componentName", this);
			string methodName;
			if (!attributes.TryGetAndRemove(context, "methodName", out methodName))
				throw new AttributeNullException("methodName", this);

			// invoke the method
			return context.InvokeMethodOnComponent<Dataset>(componentName, methodName, attributes);
		}
		#endregion
	}
}