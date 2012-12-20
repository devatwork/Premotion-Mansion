using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Data.Queries.Specifications
{
	/// <summary>
	/// Represents a specification of an property.
	/// </summary>
	public abstract class PropertySpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="PropertySpecification"/> for the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to apply this specification.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		protected PropertySpecification(string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set value
			PropertyName = propertyName;
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
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property on which this speicifaction asserts.
		/// </summary>
		public string PropertyName { get; private set; }
		#endregion
	}
}