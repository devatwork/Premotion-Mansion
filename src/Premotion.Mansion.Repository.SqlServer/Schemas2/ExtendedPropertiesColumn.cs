using System.Data;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents an extended properties column
	/// </summary>
	public class ExtendedPropertiesColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public ExtendedPropertiesColumn() : base("extendedProperties")
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
			// remove all properties starting with an underscore
			var unstoredPropertyNames = properties.Names.Where(candidate => candidate.StartsWith("_")).ToList();
			foreach (var propertyName in unstoredPropertyNames)
				properties.Remove(propertyName);

			// get the conversion service
			var conversionService = context.Nucleus.ResolveSingle<IConversionService>();

			// set the column value
			queryBuilder.AddColumnValue("extendedProperties", conversionService.Convert<byte[]>(context, properties), DbType.Binary);
		}
		#endregion
	}
}