using System.Data;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries;

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
		/// <summary>
		/// Generates an sync statements of this colum.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="command"></param>
		/// <param name="node"></param>
		/// <param name="columnText"></param>
		/// <param name="valueText"></param>
		protected override void DoToSyncStatement(IMansionContext context, SqlCommand command, Node node, StringBuilder columnText, StringBuilder valueText)
		{
			// join the two tables on ID
			columnText.Append("[id], ");
			valueText.Append(node.Pointer.Id + ", ");
		}
		#endregion
	}
}