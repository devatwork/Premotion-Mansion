using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a type table.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "typeTable")]
	public class TypeTableDescriptor : TypeDescriptor
	{
		#region Factory Methods
		/// <summary>
		/// Creates the table as described.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <returns></returns>
		public virtual Table Create(IMansionContext context, Schema schema)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (schema == null)
				throw new ArgumentNullException("schema");

			// create the type table
			var tableName = Properties.Get<string>(context, "tableName");
			return new TypeTable(tableName);
		}
		#endregion
	}
}