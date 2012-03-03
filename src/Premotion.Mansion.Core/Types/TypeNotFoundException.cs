using System;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// This exception is thrown when a type could not be found.
	/// </summary>
	public class TypeNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="name"></param>
		public TypeNotFoundException(string name) : base(string.Format("Could not found type definition for type '{0}'.", name))
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set value
			Name = name;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the type which could not be found.
		/// </summary>
		public string Name { get; private set; }
		#endregion
	}
}