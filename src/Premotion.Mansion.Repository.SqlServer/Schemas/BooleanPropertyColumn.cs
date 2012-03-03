using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represens a <see cref="bool"/> property column.
	/// </summary>
	public class BooleanPropertyColumn : PropertyColumn
	{
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="columnName">The name of the column.</param>
		/// <param name="properties"></param>
		public BooleanPropertyColumn(string propertyName, string columnName, IPropertyBag properties) : base(propertyName, columnName, properties)
		{
		}
		#endregion
		#region Overrides of PropertyColumn
		/// <summary>
		/// Normalizes the value of this column.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="input">The input which to normalize.</param>
		/// <returns>Returns the normalized value.</returns>
		protected override object DoNormalize(MansionContext context, object input)
		{
			// parse to boolean
			var conversionService = context.Nucleus.Get<IConversionService>(context);
			var flag = conversionService.Convert<bool>(context, base.DoNormalize(context, input));
			return flag;
		}
		#endregion
	}
}