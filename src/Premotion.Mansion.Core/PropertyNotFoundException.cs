using System;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// This exception is thrown when a property can not be found in a particular <see cref="IPropertyBag"/>
	/// </summary>
	public class PropertyNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="bag"></param>
		/// <param name="propertyName"></param>
		public PropertyNotFoundException(IPropertyBag bag, string propertyName)
		{
			// validate arguments
			if (bag == null)
				throw new ArgumentNullException("bag");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			Bag = bag;
			PropertyName = propertyName;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property which could not be found.
		/// </summary>
		public string PropertyName { get; private set; }
		/// <summary>
		/// Gets the property bag which did not containg the requested property.
		/// </summary>
		public IPropertyBag Bag { get; private set; }
		#endregion
	}
}