﻿using System;
using System.Data;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents the order column.
	/// </summary>
	public class OrderColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public OrderColumn() : base("order")
		{
		}
		#endregion
		#region ToStatement Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="properties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, IPropertyBag properties)
		{
			// build the select max order + 1 query only for none root nodes
			var newPointer = properties.Get<NodePointer>(context, "_newPointer");
			queryBuilder.PrependQuery("DECLARE @order int = 1");
			if (newPointer.Depth > 1)
				queryBuilder.PrependQuery(string.Format("SET @order = (SELECT ISNULL(MAX([order]) + 1, 1) FROM [Nodes] WHERE [parentId] = {0})", queryBuilder.AddParameter("parentId", newPointer.Parent.Id, DbType.Int32)));

			// add the column
			queryBuilder.AddColumnValue(PropertyName, "@order");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="record"> </param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Record record, IPropertyBag modifiedProperties)
		{
			// check if the property is not modified
			int newOrder;
			if (!modifiedProperties.TryGet(context, PropertyName, out newOrder))
				return;

			// check if the record contains a pointer
			NodePointer pointer;
			if (!record.TryGet(context, "pointer", out pointer))
				throw new InvalidOperationException("Could not update this record because it did not contain a pointer");

			// don't update order for root  nodes
			if (pointer.Depth == 1)
				return;

			// do not allow values smaller than 1
			if (newOrder < 1)
				throw new InvalidOperationException("Can not set orders smaller than 1");

			// assemble parameter
			var newOrderParameterName = queryBuilder.AddParameter("newOrder", newOrder, DbType.Int32);
			var oldOrderParameterName = queryBuilder.AddParameter("oldOrder", record.Get<int>(context, PropertyName), DbType.Int32);
			var parentIdParameterName = queryBuilder.AddParameter("parentId", pointer.Parent.Id, DbType.Int32);

			// update the orders before updating the order of the current node
			queryBuilder.PrependQuery(string.Format("UPDATE [Nodes] SET [order] = [order] + 1 WHERE (parentId = {0}) AND ([order] < {1} AND [order] >= {2})", parentIdParameterName, oldOrderParameterName, newOrderParameterName));
			queryBuilder.PrependQuery(string.Format("UPDATE [Nodes] SET [order] = [order] - 1 WHERE (parentId = {0}) AND ([order] > {1} AND [order] <= {2})", parentIdParameterName, oldOrderParameterName, newOrderParameterName));

			// update the column
			queryBuilder.AddColumnValue(PropertyName, newOrderParameterName);
		}
		#endregion
	}
}