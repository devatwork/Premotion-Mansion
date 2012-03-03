using System;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// This exception is thrown when a dataspace with a particular name can not be found on the data stack or it can not be converted to the proper type.
	/// </summary>
	public class DataspaceNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="dataspaceName"></param>
		/// <param name="targetType"></param>
		public DataspaceNotFoundException(string dataspaceName, Type targetType)
		{
			// validate arugments
			if (string.IsNullOrEmpty(dataspaceName))
				throw new ArgumentNullException("dataspaceName");
			if (targetType == null)
				throw new ArgumentNullException("targetType");

			// format the message
			message = string.Format("Could not find dataspace '{0}' on the stack or it could not be converted to '{1}'", dataspaceName, targetType);
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
			get { return message; }
		}
		#endregion
		#region Private Fields
		private readonly string message;
		#endregion
	}
}