using System;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a column which its table.
	/// </summary>
	public class TableColumnPair
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="table"></param>
		/// <param name="column"></param>
		public TableColumnPair(Table table, Column column)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");
			if (column == null)
				throw new ArgumentNullException("column");

			// set values
			Table = table;
			Column = column;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the table.
		/// </summary>
		public Table Table { get; private set; }
		/// <summary>
		/// Gets the table.
		/// </summary>
		public Column Column { get; private set; }
		#endregion
	}
}