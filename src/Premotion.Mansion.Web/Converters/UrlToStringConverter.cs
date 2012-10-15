using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Web.Converters
{
	/// <summary>
	/// Converts <see cref="Url"/> into <see cref="string"/>s.
	/// </summary>
	public class UrlToStringConverter : ConverterBase<Url, string>
	{
		#region Constructor
		///<summary>
		/// Default constructor.
		///</summary>
		public UrlToStringConverter() : base(VoteResult.MediumInterest)
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
		protected override string DoConvert(IMansionContext context, Url source, Type sourceType)
		{
			// return the full url when the hostname changed, otherwise the relative url
			return source.ToString();
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IMansionContext context, Url source, Type sourceType, string defaultValue)
		{
			return DoConvert(context, source, sourceType);
		}
		#endregion
	}
}