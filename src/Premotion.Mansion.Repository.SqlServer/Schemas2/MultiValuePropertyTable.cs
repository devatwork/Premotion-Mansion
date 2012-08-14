using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a multi-value property table.
	/// </summary>
	public class MultiValuePropertyTable : Table
	{
		#region Constructors
		/// <summary>
		/// Constructs this table with the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of this table.</param>
		public MultiValuePropertyTable(string name) : base(name)
		{
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds a property name ot this multi valued table.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>Returns this table.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public MultiValuePropertyTable Add(string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// add to the list
			propertyNames.Add(propertyName);

			// return this for chaining
			return this;
		}
		#endregion
		#region Private Fields
		private readonly List<string> propertyNames = new List<string>();
		#endregion
	}
}