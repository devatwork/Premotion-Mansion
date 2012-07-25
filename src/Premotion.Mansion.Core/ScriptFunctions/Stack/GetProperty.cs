using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Stack
{
	/// <summary>
	/// Gets a value from the given propertybag.
	/// </summary>
	[ScriptFunction("GetProperty")]
	public class GetProperty : FunctionExpression
	{
		/// <summary>
		/// Gets a value from the given propertybag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyBag">The <see cref="IPropertyBag"/> from which to get the property.</param>
		/// <param name="propertyName">The name of the property from which to get the value.</param>
		/// <returns>Returns the value of the specified property.</returns>
		public object Evaluate(IMansionContext context, IPropertyBag propertyBag, string propertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (propertyBag == null)
				throw new ArgumentNullException("propertyBag");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// get the property
			object obj;
			if (!propertyBag.TryGet(context, propertyName, out obj))
				throw new InvalidOperationException(string.Format("Does not contain property '{0}'", propertyName));
			return obj;
		}
		/// <summary>
		/// Gets a value from the script stack.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyBag">The <see cref="IPropertyBag"/> from which to get the property.</param>
		/// <param name="propertyName">The name of the property from which to get the value.</param>
		/// <param name="defaultValue">The default value which to return in case the <paramref name="propertyBag"/> does not contain <paramref name="propertyName"/>.</param>
		/// <returns>Returns the value of the specified property.</returns>
		public object Evaluate(IMansionContext context, IPropertyBag propertyBag, string propertyName, object defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (propertyBag == null)
				throw new ArgumentNullException("propertyBag");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// get the property
			object obj;
			return !propertyBag.TryGet(context, propertyName, out obj) ? defaultValue : obj;
		}
	}
}