using System;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Thrown when there is no converter available.
	/// </summary>
	public class NoConverterFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceType"></param>
		/// <param name="targetType"></param>
		public NoConverterFoundException(Type sourceType, Type targetType)
		{
			// validate arguments
			if (sourceType == null)
				throw new ArgumentNullException("sourceType");
			if (targetType == null)
				throw new ArgumentNullException("targetType");

			// set values
			this.sourceType = sourceType;
			this.targetType = targetType;
		}
		#endregion
		#region Overrides of Exception
		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <returns>
		/// The error message that explains the reason for the exception, or an empty string("").
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override string Message
		{
			get { return string.Format("No converter found which can convert '{0}' into '{1}'.", sourceType, targetType); }
		}
		#endregion
		#region Private Fields
		private readonly Type sourceType;
		private readonly Type targetType;
		#endregion
	}
}