using System;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Default implements of <see cref="RowMappingStrategy"/>.
	/// </summary>
	public class DefaultRowMappingStrategy : RowMappingStrategy
	{
		#region Constructors
		/// <summary>
		/// Private constructor.
		/// </summary>
		private DefaultRowMappingStrategy()
		{
		}
		#endregion
		#region Overrides of RowMappingStrategy
		/// <summary>
		/// Maps the properties of the row.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="row">The row which to map.</param>
		/// <returns>Returns the mapped result.</returns>
		protected override IPropertyBag DoMapRowProperties(IMansionWebContext context, IPropertyBag row)
		{
			return row;
		}
		#endregion
		#region Singletong
		/// <summary>
		/// Gets the <see cref="DefaultRowMappingStrategy"/> instance.
		/// </summary>
		public static DefaultRowMappingStrategy Instance
		{
			get { return singleton.Value; }
		}
		#endregion
		#region Private Fields
		private static readonly Lazy<DefaultRowMappingStrategy> singleton = new Lazy<DefaultRowMappingStrategy>(() => new DefaultRowMappingStrategy());
		#endregion
	}
}