using System;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts objects into strings.
	/// </summary>
	public class ObjectToStringConverter : ConverterBase<object, string>
	{
		#region Constructor
		///<summary>
		/// Default constructor.
		///</summary>
		public ObjectToStringConverter() : base(VoteResult.LowInterest)
		{
		}
		#endregion
		#region Overrides of ConverterBase<object,string>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override string DoConvert(IMansionContext context, object source, Type sourceType)
		{
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
		protected override string DoConvert(IMansionContext context, object source, Type sourceType, string defaultValue)
		{
			return source.ToString();
		}
		#endregion
	}
}