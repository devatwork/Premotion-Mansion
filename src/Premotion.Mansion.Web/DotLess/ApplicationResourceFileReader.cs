using System;
using System.IO;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using dotless.Core.Input;

namespace Premotion.Mansion.Web.DotLess
{
	/// <summary>
	/// Implements <see cref="IFileReader"/> using <see cref="IApplicationResourceService"/>.
	/// </summary>
	public class ApplicationResourceFileReader : IFileReader
	{
		#region Implementation of IFileReader
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public byte[] GetBinaryFileContents(string fileName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			// get the required references
			var context = DotLessContextHelper.GetContext();
			var resourceService = DotLessContextHelper.ApplicationResourceService;

			// parse the path
			var path = ParsePath(context, resourceService, fileName);

			// open the file and return its content
			var resource = resourceService.GetSingle(context, path);
			using (var pipe = resource.OpenForReading())
			using (var memStream = new MemoryStream())
			{
				pipe.RawStream.CopyTo(memStream);
				return memStream.ToArray();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public string GetFileContents(string fileName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			// get the required references
			var context = DotLessContextHelper.GetContext();
			var resourceService = DotLessContextHelper.ApplicationResourceService;

			// parse the path
			var path = ParsePath(context, resourceService, fileName);

			// open the file and return its content
			var resource = resourceService.GetSingle(context, path);
			using (var pipe = resource.OpenForReading())
				return pipe.Reader.ReadToEnd();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public bool DoesFileExist(string fileName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			// get the required references
			var context = DotLessContextHelper.GetContext();
			var resourceService = DotLessContextHelper.ApplicationResourceService;

			// parse the path
			var path = ParsePath(context, resourceService, fileName);

			// check if the path exists
			return resourceService.Exists(context, path);
		}
		/// <summary>
		/// Never use cache dependencies.
		/// </summary>
		public bool UseCacheDependencies
		{
			get { return false; }
		}
		#endregion
		#region Resource Methods
		/// <summary>
		/// Parses the given <paramref name="fileName"/> into a <see cref="IResourcePath"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="resourceService"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private static IResourcePath ParsePath(IMansionContext context, IResourceService resourceService, string fileName)
		{
			// check if the filename starts with the application path
			if (fileName.StartsWith(HttpContext.Current.Request.ApplicationPath))
				fileName = fileName.Substring(HttpContext.Current.Request.ApplicationPath.Length);

			// trim the file name
			fileName = fileName.Trim('/', '\\');

			// create the properties
			var properties = new PropertyBag
			                 {
			                 	{"path", fileName}
			                 };

			// parse the path
			return resourceService.ParsePath(context, properties);
		}
		#endregion
	}
}