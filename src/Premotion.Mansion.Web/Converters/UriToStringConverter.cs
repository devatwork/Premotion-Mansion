using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Web.Converters
{
	/// <summary>
	/// Converts <see cref="Uri"/> into <see cref="String"/>s.
	/// </summary>
	public class UriToStringConverter : ConverterBase<Uri, string>
	{
		#region Constructor
		///<summary>
		/// Default constructor.
		///</summary>
		public UriToStringConverter() : base(VoteResult.MediumInterest)
		{
		}
		#endregion
		#region Overrides of ConverterBase<object,string>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IContext context, Uri source, Type sourceType)
		{
			// return the full url when the hostname changed, otherwise the relative url
			return WebUtilities.StripPort(source).ToString();
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IContext context, Uri source, Type sourceType, string defaultValue)
		{
			return DoConvert(context, source, sourceType);
		}
		#endregion
	}
}