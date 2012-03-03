using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Implements the base class for converters.
	/// </summary>
	/// <typeparam name="TSource">The source type from which to convert.</typeparam>
	/// <typeparam name="TTarget">The target type from which to convert.</typeparam>
	[Exported]
	public abstract class ConverterBase<TSource, TTarget> : IConverter
	{
		#region Constructors
		/// <summary>
		/// Default constructor.
		/// </summary>
		protected ConverterBase() : this(VoteResult.MediumInterest)
		{
		}
		/// <summary>
		/// Constructor with interest weight.
		/// </summary>
		/// <param name="interestWeight">The interest weight.</param>
		protected ConverterBase(VoteResult interestWeight)
		{
			// validate arguments
			if (interestWeight == null)
				throw new ArgumentNullException("interestWeight");

			// set values
			this.interestWeight = interestWeight;
		}
		#endregion
		#region Implementation of ICandidate<in Type>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(IContext context, ConversionVotingSubject subject)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (subject == null)
				throw new ArgumentNullException("subject");
			return DoVote(context, subject);
		}
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected virtual VoteResult DoVote(IContext context, ConversionVotingSubject subject)
		{
			return SourceType.IsAssignableFrom(subject.SourceType) && subject.TargetType.IsAssignableFrom(TargetType) ? interestWeight : VoteResult.Refrain;
		}
		#endregion
		#region Converstion Methods
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		public object Convert(IContext context, object source, Type sourceType)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (sourceType == null)
				throw new ArgumentNullException("sourceType");

			// invoke template method
			return DoConvert(context, (TSource) source, sourceType);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		public object Convert(IContext context, object source, Type sourceType, object defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (sourceType == null)
				throw new ArgumentNullException("sourceType");

			// invoke template method
			return DoConvert(context, (TSource) source, sourceType, (TTarget) defaultValue);
		}
		#endregion
		#region Converstion Methods
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected abstract TTarget DoConvert(IContext context, TSource source, Type sourceType);
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected abstract TTarget DoConvert(IContext context, TSource source, Type sourceType, TTarget defaultValue);
		#endregion
		#region Properties
		/// <summary>
		/// The source type from which this converter converts.
		/// </summary>
		public Type SourceType
		{
			get { return typeof (TSource); }
		}
		/// <summary>
		/// The target type to which this converter converts.
		/// </summary>
		public Type TargetType
		{
			get { return typeof (TTarget); }
		}
		#endregion
		#region Private Fields
		private readonly VoteResult interestWeight;
		#endregion
	}
}