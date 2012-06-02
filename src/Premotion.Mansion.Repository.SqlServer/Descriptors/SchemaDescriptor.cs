using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Descriptors
{
	/// <summary>
	/// Describes a schema of an type table.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "schema")]
	public class SchemaDescriptor : TypeDescriptor
	{
		#region Properties
		/// <summary>
		/// Gets the schema.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public string GetSchema(IMansionContext context)
		{
			return Properties.Get<string>(context, "content");
		}
		#endregion
	}
}