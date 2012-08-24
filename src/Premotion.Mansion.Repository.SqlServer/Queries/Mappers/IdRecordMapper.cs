using Premotion.Mansion.Core;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Mappers
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
		/// Maps the given <paramref name="dbRecord"/> to <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="dbRecord">The <see cref="DbRecord"/> which to map.</param>
		/// <param name="properties">The <see cref="IPropertyBag"/> in which to store the mapped result.</param>
		protected override void DoMap(IMansionContext context, DbRecord dbRecord, IPropertyBag properties)
		{
			// field indices
			var idIndex = dbRecord.GetOrdinal("id");

			// set the pointer
			properties.Set("id", dbRecord.GetInt32(idIndex));
		}
		#endregion
	}
}