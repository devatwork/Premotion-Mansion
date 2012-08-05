using System.Data;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.QueryCommands.Mappers
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
		/// Maps the given <paramref name="record"/> to <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="IDataRecord"/> which to map.</param>
		/// <param name="properties">The <see cref="IPropertyBag"/> in which to store the mapped result.</param>
		protected override void DoMap(IMansionContext context, IDataRecord record, IPropertyBag properties)
		{
			// field indices
			var idIndex = record.GetOrdinal("id");
			var parentPointerIndex = record.GetOrdinal("parentPointer");
			var nameIndex = record.GetOrdinal("name");
			var parentPathIndex = record.GetOrdinal("parentPath");
			var typeIndex = record.GetOrdinal("type");
			var parentStructureIndex = record.GetOrdinal("parentStructure");

			// assemble the node pointer
			var pointer = (record.IsDBNull(parentPointerIndex) ? string.Empty : record.GetString(parentPointerIndex) + NodePointer.PointerSeparator) + record.GetInt32(idIndex);
			var structure = (record.IsDBNull(parentStructureIndex) ? string.Empty : record.GetString(parentStructureIndex) + NodePointer.StructureSeparator) + record.GetString(typeIndex);
			var path = (record.IsDBNull(parentPathIndex) ? string.Empty : record.GetString(parentPathIndex) + NodePointer.PathSeparator) + record.GetString(nameIndex);
			var nodePointer = NodePointer.Parse(pointer, structure, path);

			// set the pointer
			properties.Set("id", nodePointer.Id);
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