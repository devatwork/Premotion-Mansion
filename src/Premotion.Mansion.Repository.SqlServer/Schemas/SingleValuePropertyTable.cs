using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents a multi-value property table.
	/// </summary>
	public class SingleValuePropertyTable : Table
	{
		#region Constructors
		/// <summary>
		/// Constructs a table.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="propertyName"></param>
		public SingleValuePropertyTable(string name, string propertyName) : base(name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			PropertyName = propertyName;

			// add a column
			AddColumn(new PropertyColumn(PropertyName, "value", new PropertyBag()));
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
		protected override void DoToInsertStatement(MansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			// check if there are any properties
			var values = newProperties.Get(context, PropertyName, string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
			if (values.Length == 0)
				return;

			// loop through each value and write an insert statement
			foreach (var value in values)
			{
				// build the query
				var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

				// set column values
				valueModificationQuery.AddColumnValue("id", "@ScopeIdentity");
				valueModificationQuery.AddColumnValue("value", value, DbType.String);

				// append the query
				queryBuilder.AppendQuery(valueModificationQuery.ToInsertStatement(Name));
			}
		}
		/// <summary>
		/// Generates the update statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(MansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// check if the property is modified
			string rawModifiedValue;
			if (!modifiedProperties.TryGet(context, PropertyName, out rawModifiedValue))
				return;

			// get the current values
			var currentValues = GetCurrentValues(queryBuilder.Command, node).ToList();

			// check if there are new properties
			var modifiedValues = rawModifiedValue.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

			// get the deleted values
			var deletedValues = currentValues.Except(modifiedValues, StringComparer.OrdinalIgnoreCase);
			var newValues = modifiedValues.Except(currentValues, StringComparer.OrdinalIgnoreCase);

			// create identity parameter
			var idParameterName = queryBuilder.AddParameter("id", node.Pointer.Id, DbType.Int32);

			// generate the delete statements
			foreach (var deletedValue in deletedValues)
			{
				// build the query
				var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

				// build clause
				var valueParameterName = valueModificationQuery.AddParameter("value", deletedValue, DbType.String);
				valueModificationQuery.AppendWhereClause("[id] = " + idParameterName + " AND [value] = " + valueParameterName);

				// append the query
				queryBuilder.AppendQuery(valueModificationQuery.ToDeleteStatement(Name));
			}

			// generate the insert statements
			foreach (var newValue in newValues)
			{
				// build the query
				var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

				// set column values
				valueModificationQuery.AddColumnValue("id", idParameterName);
				valueModificationQuery.AddColumnValue("value", newValue, DbType.String);

				// append the query
				queryBuilder.AppendQuery(valueModificationQuery.ToInsertStatement(Name));
			}
		}
		/// <summary>
		/// Generates an table sync statement for this table.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="bulkContext"></param>
		/// <param name="nodes"></param>
		protected override void DoToSyncStatement(MansionContext context, BulkOperationContext bulkContext, List<Node> nodes)
		{
			// start by clearing the table
			bulkContext.Add(command =>
			                {
			                	command.CommandType = CommandType.Text;
			                	command.CommandText = string.Format("TRUNCATE TABLE [{0}]", Name);
			                });

			// loop through all the properties
			foreach (var node in nodes)
			{
				// check if there are any properties
				var values = node.Get(context, PropertyName, string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
				if (values.Length == 0)
					continue;

				// loop through each value and write an insert statement
				foreach (var value in values)
				{
					var node1 = node;
					var value1 = value;
					bulkContext.Add(command =>
					                {
					                	command.CommandType = CommandType.Text;
					                	command.CommandText = string.Format("INSERT INTO [{0}] ([id], [value]) VALUES ({1}, @{2});", Name, node1.Pointer.Id, command.AddParameter(value1));
					                });
				}
			}
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Gets the current values of this table.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		private IEnumerable<string> GetCurrentValues(IDbCommand command, Node node)
		{
			using (var selectCommand = command.Connection.CreateCommand())
			{
				selectCommand.CommandType = CommandType.Text;
				selectCommand.CommandText = string.Format("SELECT [value] FROM [{0}] WHERE [id] = '{1}'", Name, node.Pointer.Id);
				selectCommand.Transaction = command.Transaction;
				using (var reader = selectCommand.ExecuteReader())
				{
					if (reader == null)
						throw new InvalidOperationException("Something terrible happened");

					while (reader.Read())
						yield return reader.GetValue(0).ToString();
				}
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property which to store.
		/// </summary>
		private string PropertyName { get; set; }
		#endregion
	}
}