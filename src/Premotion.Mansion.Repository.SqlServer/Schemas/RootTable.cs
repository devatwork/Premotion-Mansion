using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents a table into values of one specific type are stored.
	/// </summary>
	public class RootTable : Table
	{
		#region Constructors
		/// <summary>
		/// Constructs a root table.
		/// </summary>
		/// <param name="tableName">The name of the root table.</param>
		public RootTable(string tableName) : base(tableName)
		{
			// create the columns
			Add(new IdentityColumn());
			Add(new OrderColumn());
			Add(new ExtendedPropertiesColumn());
		}
		#endregion
		#region Overrides of Table
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="properties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, IPropertyBag properties)
		{
			// create a table modification query
			var tableModificationQuery = new ModificationQueryBuilder(queryBuilder);
			foreach (var column in Columns)
				column.ToInsertStatement(context, tableModificationQuery, properties);

			// if there are no modified column add table modification query to the master query builder
			if (!tableModificationQuery.HasModifiedColumns)
				return;

			queryBuilder.PrependQuery("DECLARE @ScopeIdentity AS int");
			queryBuilder.AppendQuery(tableModificationQuery.ToInsertStatement(Name));
			queryBuilder.AppendQuery("SET @ScopeIdentity = SCOPE_IDENTITY()");
		}
		/// <summary>
		/// Generates the update statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="record"> </param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Record record, IPropertyBag modifiedProperties)
		{
			// create a table modification query
			var tableModificationQuery = new ModificationQueryBuilder(queryBuilder);
			foreach (var column in Columns)
				column.ToUpdateStatement(context, tableModificationQuery, record, modifiedProperties);

			// if there are no modified column add table modification query to the master query builder
			if (tableModificationQuery.HasModifiedColumns)
				queryBuilder.AppendQuery(tableModificationQuery.ToUpdateStatement(Name));
		}
		/// <summary>
		/// Generates an table sync statement for this table.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="bulkContext"></param>
		/// <param name="records"></param>
		protected override void DoToSyncStatement(IMansionContext context, BulkOperationContext bulkContext, List<Record> records)
		{
			// this is only invoked when there is a issue in mansion framwork
			throw new NotSupportedException("Root tables will not be synced");
		}
		#endregion
	}
}