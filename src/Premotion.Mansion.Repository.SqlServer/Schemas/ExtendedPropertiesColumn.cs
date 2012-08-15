using System.Data;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// assemble the extended properties
			var extendedProperties = new PropertyBag(node);
			extendedProperties.Merge(modifiedProperties);

			// remove all properties starting with an underscore
			var unstoredPropertyNames = extendedProperties.Names.Where(candidate => candidate.StartsWith("_")).ToList();
			foreach (var propertyName in unstoredPropertyNames)
				extendedProperties.Remove(propertyName);

			// get the conversion service
			var conversionService = context.Nucleus.ResolveSingle<IConversionService>();

			// set the column value
			queryBuilder.AddColumnValue("extendedProperties", conversionService.Convert<byte[]>(context, extendedProperties), DbType.Binary);
		}
		#endregion
	}
}