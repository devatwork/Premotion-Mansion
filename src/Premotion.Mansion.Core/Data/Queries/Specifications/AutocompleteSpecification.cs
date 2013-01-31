using System;
using System.Collections.Generic;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries.Specifications
{
	/// <summary>
	/// Specifies an autocomplete query.
	/// </summary>
	public class AutocompleteSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs the autocomplete specification using the given <paramref name="fragment"/>.
		/// </summary>
		/// <param name="propertyName">Gets the name on which to perform the autocomplete.</param>
		/// <param name="fragment">The fragment which to complete.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		public AutocompleteSpecification(string propertyName, string fragment)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (string.IsNullOrEmpty(fragment))
				throw new ArgumentNullException("fragment");

			// set values
			PropertyName = propertyName;
			Fragment = fragment;
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Gets the names of the properties used by this query component.
		/// </summary>
		/// <returns>Returns the property hints.</returns>
		protected override IEnumerable<string> DoGetPropertyHints()
		{
			return new[] {PropertyName};
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append(PropertyName).Append(":").Append(Fragment).Append("-->");
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name on which to perform the autocomplete.
		/// </summary>
		public string PropertyName { get; private set; }
		/// <summary>
		/// Gets the fragment on which to autocomplete.
		/// </summary>
		public string Fragment { get; private set; }
		#endregion
	}
}