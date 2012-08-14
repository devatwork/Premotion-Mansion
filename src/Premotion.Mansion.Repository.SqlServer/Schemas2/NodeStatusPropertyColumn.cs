using System;
using Premotion.Mansion.Core;
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="table">The <see cref="Table"/>.</param>
		/// <param name="propertyName">The name of the property.</param>
		public NodeStatusPropertyColumn(IMansionContext context, Table table, string propertyName) : base(propertyName)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// create the columns
			var approvedColumn = new BooleanPropertyColumn("approved", "approved", new PropertyBag
			                                                                       {
			                                                                       	{"allowNullValue", false},
			                                                                       	{"defaultValue", true}
			                                                                       });
			var publicationDateColumn = new DateTimePropertyColumn("publicationDate", "publicationDate", new PropertyBag
			                                                                                             {
			                                                                                             	{"allowNullValue", false},
			                                                                                             	{"expression", "{NotNull( Column.value, Now() )}"}
			                                                                                             });
			var expirationDateColumn = new DateTimePropertyColumn("expirationDate", "expirationDate", new PropertyBag
			                                                                                          {
			                                                                                          	{"allowNullValue", false},
			                                                                                          	{"expression", "{NotNull( Column.value, MaxSqlDate() )}"}
			                                                                                          });
			var archivedColumn = new BooleanPropertyColumn("archived", "archived", new PropertyBag
			                                                                       {
			                                                                       	{"allowNullValue", false},
			                                                                       	{"defaultValue", false}
			                                                                       });

			// intialize the columns
			approvedColumn.Initialize(context);
			publicationDateColumn.Initialize(context);
			expirationDateColumn.Initialize(context);
			archivedColumn.Initialize(context);

			// add marker columns
			table.Add(approvedColumn);
			table.Add(publicationDateColumn);
			table.Add(expirationDateColumn);
			table.Add(archivedColumn);
		}
		#endregion
	}
}