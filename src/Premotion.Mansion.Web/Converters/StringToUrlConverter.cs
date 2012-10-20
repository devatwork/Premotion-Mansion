using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Web.Converters
{
	/// <summary>
	/// Converts <see cref="string"/> into <see cref="Url"/>s.
	/// </summary>
	public class StringToUrlConverter : ConverterBase<string, Url>
	{
		#region Constructor
		///<summary>
		/// Default constructor.
		///</summary>
		public StringToUrlConverter() : base(VoteResult.MediumInterest)
		{
		}
		#endregion
		#region Overrides of ConverterBase<Url,string>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Url DoConvert(IMansionContext context, string source, Type sourceType)
		{
			// return the full url when the hostname changed, otherwise the relative url
			return Url.ParseUri(context.Cast<IMansionWebContext>().Request.ApplicationUrl, new Uri(source));
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override Url DoConvert(IMansionContext context, string source, Type sourceType, Url defaultValue)
		{
			return DoConvert(context, source, sourceType);
		}
		#endregion
	}
}