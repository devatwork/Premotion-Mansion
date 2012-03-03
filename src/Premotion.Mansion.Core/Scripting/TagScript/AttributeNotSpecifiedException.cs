using System;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// This exception is thrown when a required attribute is missing on a <see cref="ScriptTag"/>.
	/// </summary>
	public class AttributeNotSpecifiedException : ApplicationException
	{
		#region Constructors
		///<summary>
		///</summary>
		///<param name="attributeName"></param>
		///<param name="scriptTag"></param>
		public AttributeNotSpecifiedException(string attributeName, ScriptTag scriptTag)
		{
			// validate arguments
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");
			if (scriptTag == null)
				throw new ArgumentNullException("scriptTag");

			// format the exception message
			message = string.Format("Could not find required attribute '{0}' on tag of type '{1}'", attributeName, scriptTag.GetType());
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