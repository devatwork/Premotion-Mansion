using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.IO
{
	///<summary>
	/// This exception is thrown when a resource path can not be resolved to a resource.
	///</summary>
	public class ResourceNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="paths"></param>
		public ResourceNotFoundException(IEnumerable<IResourcePath> paths)
		{
			// validate arguments
			if (paths == null)
				throw new ArgumentNullException("paths");

			// set values
			this.paths = paths;
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
			get
			{
				var sb = new StringBuilder();
				sb.AppendLine("Could not find a resource in any of the following paths:");
				foreach (var path in paths.Select(x => x.Paths).SelectMany(x => x))
					sb.Append("- " + path);
				return sb.ToString();
			}
		}
		#endregion
		#region Private Fields
		private readonly IEnumerable<IResourcePath> paths;
		#endregion
	}
}