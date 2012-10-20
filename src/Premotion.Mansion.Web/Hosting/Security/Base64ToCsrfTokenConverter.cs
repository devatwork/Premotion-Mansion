using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using SysConvert = System.Convert;

namespace Premotion.Mansion.Web.Hosting.Security
{
	/// <summary>
	/// Handles the conversion from <see cref="string"/> to <see cref="CsrfToken"/>.
	/// </summary>
	public class Base64ToCsrfTokenConverter : ConverterBase<string, CsrfToken>
	{
		#region Overrides of ConverterBase<string,CsrfToken>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override CsrfToken DoConvert(IMansionContext context, string source, Type sourceType)
		{
			var signatureBytes = SysConvert.FromBase64String(source);
			return new CsrfToken(signatureBytes);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override CsrfToken DoConvert(IMansionContext context, string source, Type sourceType, CsrfToken defaultValue)
		{
			try
			{
				return DoConvert(context, source, sourceType);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}