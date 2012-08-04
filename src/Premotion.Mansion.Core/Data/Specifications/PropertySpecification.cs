using System;

namespace Premotion.Mansion.Core.Data.Specifications
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
		#region Properties
		/// <summary>
		/// Gets the name of the property on which this speicifaction asserts.
		/// </summary>
		public string PropertyName { get; private set; }
		#endregion
	}
}