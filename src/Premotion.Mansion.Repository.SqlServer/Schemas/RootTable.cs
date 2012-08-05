using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.QueryCommands.Mappers;

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
			AddColumn(new IdentityColumn(this));
			AddColumn(new OrderColumn());
			AddColumn(new ExtendedPropertiesColumn());
		}
		#endregion
		#region Statement Mapping Methods
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="newProperties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			// create a table modification query
			var tableModificationQuery = new ModificationQueryBuilder(queryBuilder);
			foreach (var column in Columns)
				column.ToInsertStatement(context, tableModificationQuery, newPointer, newProperties);

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
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// create a table modification query
			var tableModificationQuery = new ModificationQueryBuilder(queryBuilder);
			foreach (var column in Columns)
				column.ToUpdateStatement(context, tableModificationQuery, node, modifiedProperties);

			// if there are no modified column add table modification query to the master query builder
			if (tableModificationQuery.HasModifiedColumns)
				queryBuilder.AppendQuery(tableModificationQuery.ToUpdateStatement(Name));
		}
		/// <summary>
		/// Generates an table sync statement for this table.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="bulkContext"></param>
		/// <param name="nodes"></param>
		protected override void DoToSyncStatement(IMansionContext context, BulkOperationContext bulkContext, List<Node> nodes)
		{
			// this is only invoked when there is a issue in mansion framwork
			throw new NotSupportedException("Root tables will not be synced");
		}
		#endregion
		#region Overrides of Table
		/// <summary>
		/// Gets the <see cref="IRecordMapper"/>s of this table.
		/// </summary>
		/// <returns>Returnss the <see cref="IRecordMapper"/>s.</returns>
		protected override IEnumerable<IRecordMapper> DoGetRecordMappers(IMansionContext context)
		{
			yield return context.Nucleus.CreateInstance<NodePointerRecordMapper>();
			yield return context.Nucleus.CreateInstance<ExtendedPropertiesRecordMapper>();
		}
		#endregion
	}
}