using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Stack
{
	/// <summary>
	/// Gets a value from the script stack.
	/// </summary>
	[ScriptFunction("GetStackValue")]
	public class GetStackValue : FunctionExpression
	{
		/// <summary>
		/// Gets a value from the script stack.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="fullPropertyName">The full name of the property including the name of the dataspace in the following format: '{dataspace}.{property}'.</param>
		/// <returns>Returns the value of the specified property.</returns>
		public object Evaluate(IMansionContext context, string fullPropertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(fullPropertyName))
				throw new ArgumentNullException("fullPropertyName");

			// split the property name
			var parts = fullPropertyName.Split(',');
			if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
				throw new InvalidOperationException(string.Format("Full property name '{0}' is not a valid name", fullPropertyName));

			// return the value
			return Evaluate(context, parts[0], parts[1]);
		}
		/// <summary>
		/// Gets a value from the script stack.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="dataspace">The name of the dataspace from which to get the property.</param>
		/// <param name="property">The name of the property from which to get the value.</param>
		/// <returns>Returns the value of the specified property.</returns>
		public object Evaluate(IMansionContext context, string dataspace, string property)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(dataspace))
				throw new ArgumentNullException("dataspace");
			if (string.IsNullOrEmpty(property))
				throw new ArgumentNullException("property");

			// first get the dataspace
			IPropertyBag properties;
			if (!context.Stack.TryPeek(dataspace, out properties))
				throw new InvalidOperationException(string.Format("Could not find dataspace '{0}' on the stack", dataspace));

			// get the property
			object obj;
			if (!properties.TryGet(context, property, out obj))
				throw new InvalidOperationException(string.Format("Dataspace '{0}' does not contain property '{1}'", dataspace, property));
			return obj;
		}
		/// <summary>
		/// Gets a value from the script stack.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="dataspace">The name of the dataspace from which to get the property.</param>
		/// <param name="property">The name of the property from which to get the value.</param>
		/// <param name="defaultValue">The default value which to return in case the <paramref name="dataspace"/> does not exist or it does not contain <paramref name="property"/>.</param>
		/// <returns>Returns the value of the specified property.</returns>
		public object Evaluate(IMansionContext context, string dataspace, string property, object defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(dataspace))
				throw new ArgumentNullException("dataspace");
			if (string.IsNullOrEmpty(property))
				throw new ArgumentNullException("property");

			// first get the dataspace
			IPropertyBag properties;
			if (!context.Stack.TryPeek(dataspace, out properties))
				return defaultValue;

			// get the property
			object obj;
			return !properties.TryGet(context, property, out obj) ? defaultValue : obj;
		}
	}
}