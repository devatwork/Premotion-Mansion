using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a <see cref="IPropertyBag"/> from a given component.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveRowFromComponent")]
	public class RetrieveRowFromComponentTag : GetRowBaseTag
	{
		#region Overrides of GetRowBaseTag
		/// <summary>
		/// Gets the row.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override IPropertyBag Get(IMansionContext context, IPropertyBag attributes)
		{
			// get the component and method names
			string componentName;
			if (!attributes.TryGetAndRemove(context, "componentName", out componentName))
				throw new AttributeNullException("componentName", this);
			string methodName;
			if (!attributes.TryGetAndRemove(context, "methodName", out methodName))
				throw new AttributeNullException("methodName", this);

			// invoke the method
			return context.InvokeMethodOnComponent<IPropertyBag>(componentName, methodName, attributes);
		}
		#endregion
	}
}