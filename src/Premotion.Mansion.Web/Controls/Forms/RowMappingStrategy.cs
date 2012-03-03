using System;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Row mapping strategy for list fields.
	/// </summary>
	public abstract class RowMappingStrategy
	{
		/// <summary>
		/// Maps the properties of the row.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="row">The row which to map.</param>
		/// <returns>Returns the mapped result.</returns>
		public IPropertyBag Map(MansionWebContext context, IPropertyBag row)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (row == null)
				throw new ArgumentNullException("row");
			return DoMapRowProperties(context, row);
		}
		/// <summary>
		/// Maps the properties of the row.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="row">The row which to map.</param>
		/// <returns>Returns the mapped result.</returns>
		protected abstract IPropertyBag DoMapRowProperties(MansionWebContext context, IPropertyBag row);
	}
}