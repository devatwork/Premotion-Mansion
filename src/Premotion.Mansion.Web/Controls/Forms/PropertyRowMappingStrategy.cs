using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Maps the properties of the input.
	/// </summary>
	public class PropertyRowMappingStrategy : RowMappingStrategy
	{
		#region Constructors
		/// <summary>
		/// Constructs the property row mapping strategy.
		/// </summary>
		/// <param name="valueProperty">The name of the property reprensenting the value.</param>
		/// <param name="labelProperty">The name of the property reprensenting the label.</param>
		public PropertyRowMappingStrategy(string valueProperty, string labelProperty)
		{
			// validate arguments
			if (string.IsNullOrEmpty(valueProperty))
				throw new ArgumentNullException("valueProperty");
			if (string.IsNullOrEmpty(labelProperty))
				throw new ArgumentNullException("labelProperty");

			// set values
			this.valueProperty = valueProperty;
			this.labelProperty = labelProperty;
		}
		#endregion
		#region Overrides of RowMappingStrategy
		/// <summary>
		/// Maps the properties of the row.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="row">The row which to map.</param>
		/// <returns>Returns the mapped result.</returns>
		protected override IPropertyBag DoMapRowProperties(MansionWebContext context, IPropertyBag row)
		{
			return new PropertyBag
			       {
			       	{"value", row.Get(context, valueProperty, string.Empty)},
			       	{"label", row.Get(context, labelProperty, string.Empty)}
			       };
		}
		#endregion
		#region Private Fields
		private readonly string labelProperty;
		private readonly string valueProperty;
		#endregion
	}
}