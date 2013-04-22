using System;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Thrown when an invalid link was detected.
	/// </summary>
	public class InvalidLinkException : Exception
	{
		/// <summary>
		/// Constructs a <see cref="InvalidLinkException"/> with the given message.
		/// </summary>
		/// <param name="message">The message that describes the error. </param>
		public InvalidLinkException(string message) : base(message)
		{
		}
	}
}