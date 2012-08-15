using System.Data;
using Premotion.Mansion.Core;
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// get the parameter name
			var parameterName = queryBuilder.AddParameter("id", node.Pointer.Id, DbType.Int32);

			// nothing to update, just tell what
			queryBuilder.AppendWhereClause("[id] = " + parameterName);
		}
		#endregion
	}
}