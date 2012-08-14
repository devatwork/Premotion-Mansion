using System;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a multi-value property table.
	/// </summary>
	public class SingleValuePropertyTable : Table
	{
		#region Constructors
		/// <summary>
		/// Constructs this table with the given <paramref name="tableName"/>.
		/// </summary>
		/// <param name="tableName">The name of this table.</param>
		/// <param name="propertyName">The name of the property which to store.</param>
		public SingleValuePropertyTable(string tableName, string propertyName) : base(tableName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			PropertyName = propertyName;

			// add a column
			Add(new VirtualColumn(propertyName));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property which to store.
		/// </summary>
		public string PropertyName { get; private set; }
		#endregion
	}
}