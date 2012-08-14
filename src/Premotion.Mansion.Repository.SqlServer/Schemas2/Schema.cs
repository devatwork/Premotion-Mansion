using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Repository.SqlServer.Queries.Mappers;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents the database schema of the current type.
	/// </summary>
	public class Schema
	{
		#region Add Methods
		/// <summary>
		/// Adds the given <paramref name="table"/> to this schema.
		/// </summary>
		/// <param name="table">The <see cref="Table"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="table"/> is null.</exception>
		public void Add(Table table)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// if this is the first table this also is the root table
			if (tables.Count == 0)
				RootTable = table;

			// add to the list
			AddOrUpdateTable(table.Name, () => table, t => t);
		}
		/// <summary>
		/// Adds or updates a table.
		/// </summary>
		/// <param name="tableName">The name of the table which to update.</param>
		/// <param name="addValueFactory">The add value factory.</param>
		/// <param name="updateValueFactory">The update value factory.</param>
		/// <returns>Returns the created or updated table.</returns>
		public void AddOrUpdateTable<TTable>(string tableName, Func<TTable> addValueFactory, Func<TTable, TTable> updateValueFactory) where TTable : Table
		{
			// validate arguments
			if (string.IsNullOrEmpty(tableName))
				throw new ArgumentNullException("tableName");
			if (addValueFactory == null)
				throw new ArgumentNullException("addValueFactory");
			if (updateValueFactory == null)
				throw new ArgumentNullException("updateValueFactory");

			// add or update the table
			tables.AddOrUpdate(tableName, key => addValueFactory(), (key, value) => updateValueFactory((TTable) value));
		}
		#endregion
		#region Find Methods
		/// <summary>
		/// Finds a table and column for the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the column.</param>
		/// <returns>Returns the <see cref="TableColumnPair"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null or empty.</exception>
		/// <exception cref="InvalidOperationException">Thrown if the property could not be found within this schema.</exception>
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
		#endregion
		#region Mapper Methods
		/// <summary>
		/// Gets the <see cref="IRecordMapper"/>s for this schema.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="IRecordMapper"/>s.</returns>
		public IEnumerable<IRecordMapper> GetRecordMappers(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// TODO: factor this to the right tables
			yield return context.Nucleus.CreateInstance<IdRecordMapper>();
			yield return context.Nucleus.CreateInstance<NodePointerRecordMapper>();
			yield return context.Nucleus.CreateInstance<ExtendedPropertiesRecordMapper>();
			yield return context.Nucleus.CreateInstance<RemainderRecordMapper>();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the root <see cref="Table"/> from this schema.
		/// </summary>
		public Table RootTable { get; private set; }
		#endregion
		#region Private Fields
		private readonly ConcurrentDictionary<string, Table> tables = new ConcurrentDictionary<string, Table>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}