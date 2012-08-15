using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a table into values of one specific type are stored.
	/// </summary>
	public class TypeTable : Table
	{
		#region Constructors
		/// <summary>
		/// Constructs this table with the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of this table.</param>
		public TypeTable(string name) : base(name)
		{
			// create the columns
			Add(new JoinColumn());
		}
		#endregion
		#region Overrides of Table
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

			// if there are modified column add table modification query to the master query builder
			if (tableModificationQuery.HasModifiedColumns)
				queryBuilder.AppendQuery(tableModificationQuery.ToInsertStatement(Name));
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

			// if there are modified column add table modification query to the master query builder
			if (tableModificationQuery.HasModifiedColumns)
				queryBuilder.AppendQuery(tableModificationQuery.ToUpdateStatement(Name));
		}
		#endregion
	}
}