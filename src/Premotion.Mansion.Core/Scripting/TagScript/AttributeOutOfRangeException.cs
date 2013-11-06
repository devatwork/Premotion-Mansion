using System;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// This exception is thrown when a attribute is out of range on a <see cref="ScriptTag"/>.
	/// </summary>
	public class AttributeOutOfRangeException : ApplicationException
	{
		#region Constructors
		///<summary>
		///</summary>
		///<param name="attributeName"></param>
		///<param name="scriptTag"></param>
		public AttributeOutOfRangeException(string attributeName, ScriptTag scriptTag)
		{
			// validate arguments
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");
			if (scriptTag == null)
				throw new ArgumentNullException("scriptTag");

			// format the exception message
			message = string.Format("The attribute '{0}' is out of range on tag of type '{1}'", attributeName, scriptTag.GetType());
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