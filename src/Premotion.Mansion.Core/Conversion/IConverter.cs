using System;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Defines the public inteface for converters.
	/// </summary>
	public interface IConverter : ICandidate<ConversionVotingSubject>
	{
		#region Conversion Methods
		/// <summary>
		/// Converts the object to <see cref="TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		object Convert(IMansionContext context, object source, Type sourceType);
		/// <summary>
		/// Converts the object to <see cref="TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		object Convert(IMansionContext context, object source, Type sourceType, object defaultValue);
		#endregion
		#region Properties
		/// <summary>
		/// The source type from which this converter converts.
		/// </summary>
		Type SourceType { get; }
		/// <summary>
		/// The target type to which this converter converts.
		/// </summary>
		Type TargetType { get; }
		#endregion
	}
}