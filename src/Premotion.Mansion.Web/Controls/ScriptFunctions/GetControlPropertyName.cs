using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Controls.ScriptFunctions
{
	/// <summary>
	/// Gets the name of an property of a control.
	/// </summary>
	[ScriptFunction("GetControlPropertyName")]
	public class GetControlPropertyName : FunctionExpression
	{
		/// <summary>
		/// Gets the property of a control.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="propertyName">The name of the control property which to get.</param>
		/// <returns>Returns the page number.</returns>
		public string Evaluate(MansionContext context, string propertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// get the control
			Control control;
			if (!context.Cast<MansionWebContext>().TryFindControl(out control))
				throw new InvalidOperationException("No control is found on the stack.");

			// return the value
			return Evaluate(context, control.Definition.Id, propertyName);
		}
		/// <summary>
		/// Gets the property of a control.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="controlId">The ID of the dataset which to page.</param>
		/// <param name="propertyName">The name of the control property which to get.</param>
		/// <returns>Returns the page number.</returns>
		public string Evaluate(MansionContext context, string controlId, string propertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controlId))
				throw new ArgumentNullException("controlId");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// return the page size
			return controlId + "_" + propertyName;
		}
	}
}