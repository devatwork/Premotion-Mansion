using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Premotion.Mansion.Core.IO.Windows
{
	/// <summary>
	/// Implements <see cref="IContentResourceService"/> for Windows based applications.
	/// </summary>
	public class WindowsContentResourceService : IContentResourceService
	{
		#region Constructors
		/// <summary>
		/// Constructs the windows content resource service.
		/// </summary>
		/// <param name="physicalBasePath">The physical base path of all resources.</param>
		/// <param name="relativeBasePath">The relative base path.</param>
		public WindowsContentResourceService(string physicalBasePath, string relativeBasePath)
		{
			// validate arguments
			if (string.IsNullOrEmpty(physicalBasePath))
				throw new ArgumentNullException("physicalBasePath");
			if (string.IsNullOrEmpty(relativeBasePath))
				throw new ArgumentNullException("relativeBasePath");

			// get the directory info
			var physicalBaseDirectory = new DirectoryInfo(physicalBasePath);
			if (!physicalBaseDirectory.Exists)
				throw new ArgumentException(string.Format("Physical base path '{0}' does not exist", physicalBaseDirectory.FullName), "physicalBasePath");

			// get the directory info
			var physicalDirectory = new DirectoryInfo(ResourceUtils.Combine(physicalBasePath, relativeBasePath));
			if (!physicalBaseDirectory.Exists)
				throw new ArgumentException(string.Format("Physical path '{0}' does not exist", physicalDirectory.FullName), "relativeBasePath");

			// check if the directory is writeable
			if (!physicalDirectory.IsWriteable())
				throw new InvalidOperationException(string.Format("Physical path '{0}' is not writeable, please check permissions", physicalDirectory.FullName));

			// set the value
			this.physicalBasePath = physicalBaseDirectory.FullName;
			this.relativeBasePath = relativeBasePath;
		}
		#endregion
		#region Nested type: ContentResourcePath
		/// <summary>
		/// Implements <see cref="IResourcePath"/> for <see cref="IContentResourceService"/>.
		/// </summary>
		private class ContentResourcePath : IResourcePath
		{
			#region Constructors
			/// <summary>
			/// Constructs a content resource path.
			/// </summary>
			/// <param name="relativePath">The relative path to the content resource.</param>
			public ContentResourcePath(string relativePath)
			{
				// validate arguments
				if (string.IsNullOrEmpty(relativePath))
					throw new ArgumentNullException("relativePath");

				// store the values
				Paths = new[] {relativePath};
			}
			#endregion
			#region Implementation of IResourcePath
			/// <summary>
			/// Gets a flag indicating whether this resource is overridable or not.
			/// </summary>
			public bool Overridable
			{
				get { return false; }
			}
			/// <summary>
			/// Gets the relative path to this resource.
			/// </summary>
			public IEnumerable<string> Paths { get; private set; }
			#endregion
		}
		#endregion
		#region Implementation of IContentResourceService
		/// <summary>
		/// Opens the resource using the specified path. This will create the resource if it does not already exist.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The <see cref="IResourcePath"/> identifying the resource.</param>
		/// <returns>Returns the <see cref="IResource"/>.</returns>
		public IResource GetResource(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");

			return new FileResource(new FileInfo(ResourceUtils.Combine(physicalBasePath, relativeBasePath, path.Paths.Single())), path);
		}
		#endregion
		#region Implementation of IResourceService
		/// <summary>
		/// Checks whether a resource exists at the specified paths.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resource.</param>
		/// <returns>Returns true when a resource exists, otherwise false.</returns>
		public bool Exists(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");

			return File.Exists(ResourceUtils.Combine(physicalBasePath, relativeBasePath, path.Paths.Single()));
		}
		/// <summary>
		/// Parses the <paramref name="properties"/> into a <see cref="IResourcePath"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties which to parse.</param>
		/// <returns>Returns the parsed <see cref="IResourcePath"/>.</returns>
		public IResourcePath ParsePath(IMansionContext context, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// get the resource base path
			var categoryBasePath = properties.Get(context, "category", "Temp");

			// check if it is an existing resource
			string relativePath;
			if (properties.TryGet(context, "relativePath", out relativePath))
				return new ContentResourcePath(ResourceUtils.Combine(categoryBasePath, relativePath));

			// check if it is a new file name
			string fileName;
			if (properties.TryGet(context, "fileName", out fileName))
			{
				// get the current date
				var today = DateTime.Today;

				// get the file base name and extension
				var fileBaseName = Path.GetFileNameWithoutExtension(fileName);
				var fileExtension = Path.GetExtension(fileName);

				// make sure the file name is unique
				var index = 0;
				while (File.Exists(ResourceUtils.Combine(physicalBasePath, relativeBasePath, categoryBasePath, today.Year.ToString(CultureInfo.InvariantCulture), today.Month.ToString(CultureInfo.InvariantCulture), fileBaseName + index + fileExtension)))
					index++;

				// create the resource path
				return new ContentResourcePath(ResourceUtils.Combine(categoryBasePath, today.Year.ToString(CultureInfo.InvariantCulture), today.Month.ToString(CultureInfo.InvariantCulture), fileBaseName + index + fileExtension));
			}

			// unkonwn type
			throw new InvalidOperationException("Could not identify resource path");
		}
		/// <summary>
		/// Gets the first and most important relative path of <paramref name="resourcePath"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resourcePath">The <see cref="IResourcePath"/>.</param>
		/// <returns>Returns a string version of the most important relative path.</returns>
		public string GetFirstRelativePath(IMansionContext context, IResourcePath resourcePath)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resourcePath == null)
				throw new ArgumentNullException("resourcePath");

			// just return the first path
			return resourcePath.Paths.First();
		}
		#endregion
		#region Private Fields
		private readonly string physicalBasePath;
		private readonly string relativeBasePath;
		#endregion
	}
}