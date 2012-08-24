using Premotion.Mansion.Core;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Mappers
{
	/// <summary>
	/// Maps the remaining properties to the record.
	/// </summary>
	public class RemainderRecordMapper : RecordMapper
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public RemainderRecordMapper() : base(10)
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
			// set all the column values as properties
			foreach (var ordinals in dbRecord.GetUnreadOrdinals())
			{
				// if the column is empty remove the value from the properties, otherwise set the value from the column
				if (dbRecord.IsDBNull(ordinals))
				{
					object obj;
					properties.TryGetAndRemove(context, dbRecord.GetName(ordinals), out obj);
				}
				else
					properties.Set(dbRecord.GetName(ordinals), dbRecord.GetValue(ordinals));
			}
		}
		#endregion
	}
}