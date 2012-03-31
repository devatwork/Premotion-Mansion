using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a property table.
	/// </summary>
	[Named(typeof (TypeDescriptor), Constants.DescriptorNamespaceUri, "singleValuePropertyTable")]
	public class SingleValuePropertyTableDescriptor : PropertyTableDescriptor
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
		public override void CreateTableInSchema(IMansionContext context, Schema schema, string propertyName, bool isOwner)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// create or update the table
			var tableName = Properties.Get<string>(context, "tableName");
			schema.AddOrUpdateTable(tableName, () => new SingleValuePropertyTable(tableName, propertyName), table => { throw new InvalidOperationException(string.Format("Single value property tables can only be used once, see {0}.{1}", TypeDefinition.Name, propertyName)); }, isOwner);
		}
		#endregion
	}
}