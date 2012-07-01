using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents a path to a relative resource.
	/// </summary>
	public class RelativeResourcePath : IResourcePath
	{
		#region Constructors
		/// <summary>
		/// Constructs a relative resource path.
		/// </summary>
		/// <param name="path">The path to the resource.</param>
		/// <param name="overridable">Flag indicating whether this resource path is overridable.</param>
		public RelativeResourcePath(string path, bool overridable)
		{
			// validate arguments
			if (path == null)
				throw new ArgumentNullException("path");

			// set values
			Paths = new[] {path};
			Overridable = overridable;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the relative path to this resource.
		/// </summary>
		public IEnumerable<string> Paths { get; private set; }
		/// <summary>
		/// Gets a flag indicating whether this resource is overridable or not.
		/// </summary>
		public bool Overridable { get; private set; }
		#endregion
	}
}