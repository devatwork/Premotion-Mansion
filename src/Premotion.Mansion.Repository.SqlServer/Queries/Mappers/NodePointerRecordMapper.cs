using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Mappers
{
	/// <summary>
	/// Implements the <see cref="IRecordMapper"/> for <see cref="NodePointer"/>s.
	/// </summary>
	public class NodePointerRecordMapper : RecordMapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public NodePointerRecordMapper() : base(500)
		{
		}
		#endregion
		#region Overrides of RecordMapper
		/// <summary>
		/// Maps the given <paramref name="dbRecord"/> to <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="dbRecord">The <see cref="DbRecord"/> which to map.</param>
		/// <param name="properties">The <see cref="IPropertyBag"/> in which to store the mapped result.</param>
		protected override void DoMap(IMansionContext context, DbRecord dbRecord, IPropertyBag properties)
		{
			// field indices
			var idIndex = dbRecord.GetOrdinal("id");
			var parentPointerIndex = dbRecord.GetOrdinal("parentPointer");
			var nameIndex = dbRecord.GetOrdinal("name");
			var parentPathIndex = dbRecord.GetOrdinal("parentPath");
			var typeIndex = dbRecord.GetOrdinal("type");
			var parentStructureIndex = dbRecord.GetOrdinal("parentStructure");

			// assemble the node pointer
			var pointer = (dbRecord.IsDBNull(parentPointerIndex) ? string.Empty : dbRecord.GetString(parentPointerIndex) + NodePointer.PointerSeparator) + dbRecord.GetInt32(idIndex);
			var structure = (dbRecord.IsDBNull(parentStructureIndex) ? string.Empty : dbRecord.GetString(parentStructureIndex) + NodePointer.StructureSeparator) + dbRecord.GetString(typeIndex);
			var path = (dbRecord.IsDBNull(parentPathIndex) ? string.Empty : dbRecord.GetString(parentPathIndex) + NodePointer.PathSeparator) + dbRecord.GetString(nameIndex);
			var nodePointer = NodePointer.Parse(pointer, structure, path);

			// set the pointer
			properties.Set("pointer", nodePointer);
			properties.Set("path", nodePointer.PathString);
			properties.Set("structure", nodePointer.StructureString);
			properties.Set("name", nodePointer.Name);
			properties.Set("type", nodePointer.Type);
			if (nodePointer.HasParent)
			{
				properties.Set("parentPointer", nodePointer.Parent);
				properties.Set("parentId", nodePointer.Parent.Id);
			}
		}
		#endregion
	}
}