using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Repository.SqlServer.QueryCommands.Mappers
{
	/// <summary>
	/// Implements the extended properties <see cref="RecordMapper"/>.
	/// </summary>
	public class ExtendedPropertiesRecordMapper : RecordMapper
	{
		#region Constructors
		/// <summary>
		/// Constucts the <see cref="ExtendedPropertiesRecordMapper"/>.
		/// </summary>
		/// <param name="conversionService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ExtendedPropertiesRecordMapper(IConversionService conversionService) : base(1000)
		{
			// validate arguments
			if (conversionService == null)
				throw new ArgumentNullException("conversionService");

			// set value
			this.conversionService = conversionService;
		}
		#endregion
		#region Overrides of RecordMapper
		/// <summary>
		/// Maps the given <paramref name="record"/> to <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to map.</param>
		/// <param name="properties">The <see cref="IPropertyBag"/> in which to store the mapped result.</param>
		protected override void DoMap(IMansionContext context, Record record, IPropertyBag properties)
		{
			// get the index of the column
			var extendedPropertiesIndex = record.GetOrdinal("extendedProperties");

			// check if there are no extended properties
			if (record.IsDBNull(extendedPropertiesIndex))
				return;

			// get the extended properties
			var extendedPropertiesLength = record.GetBytes(extendedPropertiesIndex, 0, null, 0, 0);
			var serializedProperties = new byte[extendedPropertiesLength];
			record.GetBytes(extendedPropertiesIndex, 0, serializedProperties, 0, serializedProperties.Length);

			// deserialize
			var deserializedProperties = conversionService.Convert<IPropertyBag>(context, serializedProperties);

			// merge the deserialized properties
			properties.Merge(deserializedProperties);
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}