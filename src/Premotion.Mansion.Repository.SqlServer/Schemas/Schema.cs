using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents the schema for a specific type.
	/// </summary>
	public class Schema
	{
		#region Methods
		/// <summary>
		/// Adds a table to this schema.
		/// </summary>
		/// <param name="table">The tatble which to add.</param>
		/// <param name="isOwned">Flag indicating whether the table is owned by this type.</param>
		public void AddTable(Table table, bool isOwned)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			if (isOwned)
				ownedTableNames.Add(table.Name);

			if (!tables.TryAdd(table.Name, table))
				throw new InvalidOperationException("Could not add table to schema");

			// check for root table
			if (table is RootTable)
			{
				// multiple root tables are not supported
				if (!string.IsNullOrEmpty(rootTableName))
					throw new InvalidOperationException("Multiple root tables are set. Schema is invalid");
				rootTableName = table.Name;
				tableNames.Insert(0, table.Name);
			}
			else
				tableNames.Add(table.Name);
		}
		/// <summary>
		/// Adds or updates a table.
		/// </summary>
		/// <param name="tableName">The name of the table which to update.</param>
		/// <param name="addValueFactory">The add value factory.</param>
		/// <param name="updateValueFactory">The update value factory.</param>
		/// <param name="isOwned">Flag indicating whether the table is owned by this type.</param>
		/// <returns>Returns the created or updated table.</returns>
		public void AddOrUpdateTable<TTable>(string tableName, Func<TTable> addValueFactory, Func<TTable, TTable> updateValueFactory, bool isOwned) where TTable : Table
		{
			// validate arguments
			if (string.IsNullOrEmpty(tableName))
				throw new ArgumentNullException("tableName");
			if (addValueFactory == null)
				throw new ArgumentNullException("addValueFactory");
			if (updateValueFactory == null)
				throw new ArgumentNullException("updateValueFactory");

			// add or update the table
			tables.AddOrUpdate(tableName, key =>
			                              {
			                              	if (isOwned)
			                              		ownedTableNames.Add(tableName);
			                              	tableNames.Add(tableName);
			                              	return addValueFactory();
			                              }, (key, value) => updateValueFactory((TTable) value));
		}
		/// <summary>
		/// Finds a table by it's column name.
		/// </summary>
		/// <param name="propertyName">The name of the column.</param>
		/// <returns>Returns the table.</returns>
		public TableColumnPair FindTableAndColumn(string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// find the column
			var tableAndColumn = (from candidateTable in tables.Values
			                      from candidateColumn in candidateTable.Columns
			                      where propertyName.Equals(candidateColumn.PropertyName, StringComparison.OrdinalIgnoreCase)
			                      select new TableColumnPair(candidateTable, candidateColumn)).FirstOrDefault();

			// check if a table column pair was not found
			if (tableAndColumn == null)
				throw new InvalidOperationException(string.Format("Could not find a table which contains the property '{0}'", propertyName));

			return tableAndColumn;
		}
		/// <summary>
		/// Tries to find the <paramref name="column"/> for the specified <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="column">The <see cref="PropertyColumn"/> found.</param>
		/// <returns>Returns true when <paramref name="column"/> could be found, otherwise false.</returns>
		public bool TryFindPropertyColumn(string propertyName, out PropertyColumn column)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// find the column
			column = (from candidateTable in tables.Values
			          from candidateColumn in candidateTable.Columns
			          where candidateColumn is PropertyColumn && propertyName.Equals(candidateColumn.PropertyName, StringComparison.OrdinalIgnoreCase)
			          select (PropertyColumn) candidateColumn).FirstOrDefault();

			return column != null;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the root table of this schema.
		/// </summary>
		public Table RootTable
		{
			get
			{
				Table rootTable;
				if (string.IsNullOrEmpty(rootTableName) || !tables.TryGetValue(rootTableName, out rootTable))
					throw new InvalidOperationException("Not root table in schema");
				return rootTable;
			}
		}
		/// <summary>
		/// Gets the tables in this schema.
		/// </summary>
		public IEnumerable<Table> Tables
		{
			get { return tableNames.Select(tableName => tables[tableName]); }
		}
		/// <summary>
		/// Gets the tables in this schema.
		/// </summary>
		public IEnumerable<Table> TypeTables
		{
			get { return tableNames.Select(tableName => tables[tableName]).Where(table => table is TypeTable); }
		}
		/// <summary>
		/// Gets the <see cref="Table"/> in owned by the type described by this schema.
		/// </summary>
		public IEnumerable<Table> OwnedTables
		{
			get { return ownedTableNames.Where(candidate => !rootTableName.Equals(candidate, StringComparison.OrdinalIgnoreCase)).Select(tableName => tables[tableName]); }
		}
		#endregion
		#region Private Fields
		private readonly List<string> ownedTableNames = new List<string>();
		private readonly List<string> tableNames = new List<string>();
		private readonly ConcurrentDictionary<string, Table> tables = new ConcurrentDictionary<string, Table>(StringComparer.OrdinalIgnoreCase);
		private string rootTableName;
		#endregion
	}
}