﻿using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a join between two tables.
	/// </summary>
	public class JoinColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public JoinColumn() : base("join")
		{
		}
		#endregion
		#region Overrides of Column
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="properties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag properties)
		{
			// join the two tables on ID
			queryBuilder.AddColumnValue("id", "@ScopeIdentity");
		}
		#endregion
	}
}