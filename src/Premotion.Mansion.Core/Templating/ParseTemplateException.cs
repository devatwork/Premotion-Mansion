using System;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Thrown when a <see cref="IResource"/> can not be parsed into a <see cref="ITemplate"/>.
	/// </summary>
	public class ParseTemplateException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="innerException"></param>
		public ParseTemplateException(IResource resource, Exception innerException) : base(string.Empty, innerException)
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