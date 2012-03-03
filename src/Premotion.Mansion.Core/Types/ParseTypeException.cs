using System;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Thrown when a type can not be parsed.
	/// </summary>
	public class ParseTypeException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="innerException"></param>
		public ParseTypeException(IResource resource, Exception innerException) : base(string.Empty, innerException)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");

			// set values
			Resource = resource;
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