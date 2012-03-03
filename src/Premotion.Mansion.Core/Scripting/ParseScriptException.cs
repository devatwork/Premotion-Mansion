using System;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Scripting
{
	/// <summary>
	/// This exception is thrown when a <see cref="IResource"/> could not be parsed into a <see cref="IScript"/>.
	/// </summary>
	public class ParseScriptException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="innerException"></param>
		public ParseScriptException(IResource resource, Exception innerException) : base(string.Empty, innerException)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");

			// set values
			Resource = resource;
		}
		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		public ParseScriptException(string message) : base(message)
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the resource which couldn't be parsed properly.
		/// </summary>
		public IResource Resource { get; private set; }
		#endregion
	}
}