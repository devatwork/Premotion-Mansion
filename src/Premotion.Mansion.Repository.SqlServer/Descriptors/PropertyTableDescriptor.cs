using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Descriptors
{
	/// <summary>
	/// Represents the base class for all property tables.
	/// </summary>
	public abstract class PropertyTableDescriptor : TypeDescriptor
	{
		#region Factory Methods
		/// <summary>
		/// Creates the property table as described.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <param name="propertyName"></param>
		/// <param name="isOwner"></param>
		/// <returns></returns>
		public abstract void CreateTableInSchema(IMansionContext context, Schema schema, string propertyName, bool isOwner);
		#endregion
	}
}