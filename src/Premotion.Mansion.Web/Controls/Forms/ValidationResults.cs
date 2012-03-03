using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Contains the result of an validation.
	/// </summary>
	public class ValidationResults
	{
		#region Methods
		/// <summary>
		/// Adds a validation result to this summary.
		/// </summary>
		/// <param name="rule">The <see cref="ValidationRule"/> which caused the result.</param>
		/// <param name="control">The <see cref="FormControl"/> which caused the result.</param>
		public void AddResult(ValidationRule rule, FormControl control)
		{
			// validate arguments
			if (rule == null)
				throw new ArgumentNullException("rule");
			if (control == null)
				throw new ArgumentNullException("control");

			// create the result
			results.Add(new ValidationResult(rule, control));
		}
		/// <summary>
		/// Clears the validation results.
		/// </summary>
		public void Clear()
		{
			results.Clear();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether the validation was succesful or not.
		/// </summary>
		public bool IsValid
		{
			get { return results.Count == 0; }
		}
		/// <summary>
		/// Gets the <see cref="ValidationResult"/>s.
		/// </summary>
		public IEnumerable<ValidationResult> Results
		{
			get { return results; }
		}
		#endregion
		#region Private Fields
		private readonly List<ValidationResult> results = new List<ValidationResult>();
		#endregion
	}
}