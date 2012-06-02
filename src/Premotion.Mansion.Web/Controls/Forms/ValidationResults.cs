using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;

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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="message">The message.</param>
		/// <param name="control">The <see cref="FormControl"/> which caused the result.</param>
		public void AddResult(IMansionContext context, string message, Control control)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(message))
				throw new ArgumentNullException("message");
			if (control == null)
				throw new ArgumentNullException("control");

			// create the result
			results.Add(new ValidationResult(context, message, control));
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