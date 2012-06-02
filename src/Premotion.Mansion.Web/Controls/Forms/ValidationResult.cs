using System;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Represents a single validation result.
	/// </summary>
	public class ValidationResult
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"> </param>
		/// <param name="message"></param>
		/// <param name="control"></param>
		public ValidationResult(IMansionContext context, string message, Control control)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(message))
				throw new ArgumentNullException("message");
			if (control == null)
				throw new ArgumentNullException("control");

			// set values
			ControlName = control.Definition.Properties.Get(context, "label", control.Definition.Properties.Get<string>(context, "name"));
			Message = message;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the control.
		/// </summary>
		public string ControlName { get; private set; }
		/// <summary>
		/// Gets the message.
		/// </summary>
		public string Message { get; private set; }
		#endregion
	}
}