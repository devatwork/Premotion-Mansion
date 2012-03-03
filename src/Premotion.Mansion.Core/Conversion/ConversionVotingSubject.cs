using System;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Represents the conversion voting subject.
	/// </summary>
	public class ConversionVotingSubject
	{
		#region Constructors
		/// <summary>
		/// Constructs a conversion voting subject.
		/// </summary>
		/// <param name="sourceType">the source type from which to convert.</param>
		/// <param name="targetType">the target type to which to convert.</param>
		public ConversionVotingSubject(Type sourceType, Type targetType)
		{
			// validate arguments
			if (sourceType == null)
				throw new ArgumentNullException("sourceType");
			if (targetType == null)
				throw new ArgumentNullException("targetType");

			// set values
			SourceType = sourceType;
			TargetType = targetType;
		}
		#endregion
		#region Properties
		/// <summary>
		/// The source type from which to convert.
		/// </summary>
		public Type SourceType { get; private set; }
		/// <summary>
		/// The target type to which to convert.
		/// </summary>
		public Type TargetType { get; private set; }
		#endregion
	}
}