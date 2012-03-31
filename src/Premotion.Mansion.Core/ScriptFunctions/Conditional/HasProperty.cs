using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Check whether a <see cref="IPropertyBag"/> has a specific property.
	/// </summary>
	[ScriptFunction("HasProperty")]
	public class HasProperty : FunctionExpression
	{
		/// <summary>
		/// Check whether a <paramref name="bag"/> contains <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="bag">The haystack in which to look.</param>
		/// <param name="propertyName">The name of the property which to check.</param>
		/// <returns>Returns true when <paramref name="bag"/> contains <paramref name="propertyName"/>, otherwise false.</returns>
		public bool Evaluate(IMansionContext context, IPropertyBag bag, string propertyName)
		{
			return bag.Contains(propertyName);
		}
	}
}