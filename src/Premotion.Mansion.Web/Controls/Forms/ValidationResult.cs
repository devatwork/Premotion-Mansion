using System;

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
		/// <param name="rule"></param>
		/// <param name="control"></param>
		public ValidationResult(ValidationRule rule, FormControl control)
		{
			// validate arguments
			if (rule == null)
				throw new ArgumentNullException("rule");
			if (control == null)
				throw new ArgumentNullException("control");

			// set values
			Rule = rule;
			Control = control;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="ValidationRule"/> which caused this result.
		/// </summary>
		public ValidationRule Rule { get; private set; }
		/// <summary>
		/// Gets the <see cref="FormControl"/> in which the validation was performed.
		/// </summary>
		public FormControl Control { get; private set; }
		#endregion
	}
}