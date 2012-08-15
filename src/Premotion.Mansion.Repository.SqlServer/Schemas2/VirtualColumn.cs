using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a virtual <see cref="Column"/>.
	/// </summary>
	public class VirtualColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="propertyName"></param>
		public VirtualColumn(string propertyName) : base(propertyName)
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
			// do  nothing
		}
		#endregion
	}
}