using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Controls.ScriptFunctions
{
	/// <summary>
	/// Gets the property of a control.
	/// </summary>
	[ScriptFunction("GetControlPropertyValue")]
	public class GetControlPropertyValue : FunctionExpression
	{
		/// <summary>
		/// Gets the property of a control.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="propertyName">The name of the control property which to get.</param>
		/// <returns>Returns the page number.</returns>
		public object Evaluate(MansionContext context, string propertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the control
			Control control;
			if (!context.Cast<MansionWebContext>().TryFindControl(out control))
				throw new InvalidOperationException("No control is found on the stack.");

			// return the value
			return Evaluate(context, control.Definition.Id, propertyName, null);
		}
		/// <summary>
		/// Gets the property of a control.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="propertyName">The name of the control property which to get.</param>
		/// <param name="defaultValue">The default page number.</param>
		/// <returns>Returns the page number.</returns>
		public object Evaluate(MansionContext context, string propertyName, object defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the control
			Control control;
			if (!context.Cast<MansionWebContext>().TryFindControl(out control))
				throw new InvalidOperationException("No control is found on the stack.");

			// return the value
			return Evaluate(context, control.Definition.Id, propertyName, defaultValue);
		}
		/// <summary>
		/// Gets the property of a control.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="controlId">The ID of the dataset which to page.</param>
		/// <param name="propertyName">The name of the control property which to get.</param>
		/// <param name="defaultValue">The default page number.</param>
		/// <returns>Returns the page number.</returns>
		public object Evaluate(MansionContext context, string controlId, string propertyName, object defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controlId))
				throw new ArgumentNullException("controlId");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			IPropertyBag dataspace;
			if (!context.Stack.TryPeek("Post", out dataspace) || dataspace.Count == 0)
			{
				if (!context.Stack.TryPeek("Get", out dataspace) || dataspace.Count == 0)
					return defaultValue;
			}

			// return the page size
			return dataspace.Get(context, controlId + "_" + propertyName, defaultValue);
		}
	}
}