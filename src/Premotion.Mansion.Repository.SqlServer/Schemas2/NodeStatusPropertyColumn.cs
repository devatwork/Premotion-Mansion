using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represens a <see cref="NodeStatus"/> property column.
	/// </summary>
	public class NodeStatusPropertyColumn : Column
	{
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="table">The <see cref="Table"/>.</param>
		/// <param name="propertyName">The name of the property.</param>
		public NodeStatusPropertyColumn(Table table, string propertyName) : base(propertyName)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// add marker columns
			table.Add(new BooleanPropertyColumn("approved", "approved", new PropertyBag()));
			table.Add(new DateTimePropertyColumn("publicationDate", "publicationDate", new PropertyBag()));
			table.Add(new DateTimePropertyColumn("expirationDate", "expirationDate", new PropertyBag()));
			table.Add(new BooleanPropertyColumn("archived", "archived", new PropertyBag()));
		}
		#endregion
	}
}