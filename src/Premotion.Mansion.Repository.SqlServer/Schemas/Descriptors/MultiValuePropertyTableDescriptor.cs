using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a multi-value property table.
	/// </summary>
	[Named(typeof (TypeDescriptor), Constants.DescriptorNamespaceUri, "multiValuePropertyTable")]
	public class MultiValuePropertyTableDescriptor : PropertyTableDescriptor
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
			schema.AddOrUpdateTable(tableName, () => new MultiValuePropertyTable(tableName).AddProperty(propertyName), table => table.AddProperty(propertyName), isOwner);
		}
		#endregion
	}
}