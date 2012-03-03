using System;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// This exception is thrown when a property could not be found.
	/// </summary>
	public class PropertyNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="type"></param>
		/// <param name="name"></param>
		public PropertyNotFoundException(ITypeDefinition type, string name)
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set value
			Type = type;
			Name = name;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property which could not be found.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the type which was checked for the property.
		/// </summary>
		public ITypeDefinition Type { get; private set; }
		#endregion
	}
}