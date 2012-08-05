using System.Data;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Repository.SqlServer.QueryCommands.Mappers
{
	/// <summary>
	/// Implements the <see cref="IRecordMapper"/> for IDs.
	/// </summary>
	public class IdRecordMapper : RecordMapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public IdRecordMapper() : base(750)
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

			// set the pointer
			properties.Set("id", record.GetInt32(idIndex));
		}
		#endregion
	}
}