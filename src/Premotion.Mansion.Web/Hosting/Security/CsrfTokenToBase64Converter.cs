using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using SysConvert = System.Convert;

namespace Premotion.Mansion.Web.Hosting.Security
{
	/// <summary>
	/// Handles the conversion from <see cref="CsrfToken"/> to <see cref="string"/>.
	/// </summary>
	public class CsrfTokenToBase64Converter : ConverterBase<CsrfToken, string>
	{
		#region Overrides of ConverterBase<CsrfToken,string>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IMansionContext context, CsrfToken source, Type sourceType)
		{
			return SysConvert.ToBase64String(source.Signature);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IMansionContext context, CsrfToken source, Type sourceType, string defaultValue)
		{
			throw new NotSupportedException();
		}
		#endregion
	}
}