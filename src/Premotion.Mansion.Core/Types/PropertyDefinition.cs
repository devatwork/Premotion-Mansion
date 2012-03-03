using System;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Implements <see cref="IPropertyDefinition"/>
	/// </summary>
	public class PropertyDefinition : DescripteeBase, IPropertyDefinition
	{
		#region Constructors
		/// <summary>
		/// Constructs a new property definition.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		public PropertyDefinition(string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set values
			Name = name;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this property.
		/// </summary>
		public string Name { get; private set; }
		#endregion
	}
}