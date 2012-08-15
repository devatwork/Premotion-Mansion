using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a table of a schema.
	/// </summary>
	public abstract class Table
	{
		#region Constructors
		/// <summary>
		/// Constructs this table with the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of this table.</param>
		protected Table(string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set properties
			Name = name;
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds the <paramref name="column"/> to this table.
		/// </summary>
		/// <param name="column">The <see cref="Column"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="column"/> is null.</exception>
		public void Add(Column column)
		{
			// validate arguments
			if (column == null)
				throw new ArgumentNullException("column");

			// add the column
			columns.Add(column);
		}
		#endregion
		#region Statement Mapping Methods
		/// <summary>
		/// Generates a statement which joins this table to the given <paramref name="rootTable"/>/
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="rootTable">The root <see cref="Schemas.Table"/> to which to join this table.</param>
		/// <param name="command">The <see cref="SqlCommand"/>.</param>
		/// <returns>Returns the join statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="rootTable"/> is null.</exception>
		public string ToJoinStatement(IMansionContext context, Table rootTable, SqlCommand command)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (rootTable == null)
				throw new ArgumentNullException("rootTable");
			if (command == null)
				throw new ArgumentNullException("command");

			// invoke template method
			return DoToJoinStatement(context, rootTable, command);
		}
		/// <summary>
		/// Generates a statement which joins this table to the given <paramref name="rootTable"/>/
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="rootTable">The root <see cref="Schemas.Table"/> to which to join this table.</param>
		/// <param name="command">The <see cref="SqlCommand"/>.</param>
		/// <returns>Returns the join statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="rootTable"/> is null.</exception>
		protected virtual string DoToJoinStatement(IMansionContext context, Table rootTable, SqlCommand command)
		{
			return string.Format("INNER JOIN [{0}] ON [{0}].[id] = [{1}].[id]", Name, rootTable.Name);
		}
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="newProperties"></param>
		public void ToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (newPointer == null)
				throw new ArgumentNullException("newPointer");
			if (newProperties == null)
				throw new ArgumentNullException("newProperties");

			// invoke template method
			DoToInsertStatement(context, queryBuilder, newPointer, newProperties);
		}
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="newProperties"></param>
		protected virtual void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			throw new NotSupportedException();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this table.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the <see cref="Column"/>s in this table.
		/// </summary>
		public IEnumerable<Column> Columns
		{
			get { return columns; }
		}
		#endregion
		#region Private Fields
		private readonly List<Column> columns = new List<Column>();
		#endregion
	}
}