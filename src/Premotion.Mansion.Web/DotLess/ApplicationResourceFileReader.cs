using System;
using System.IO;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Web.Hosting.AspNet;
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
			var context = GetContext(HttpContext.Current);
			var resourceService = ResourceServiceResolver.Value;

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
			var context = GetContext(HttpContext.Current);
			var resourceService = ResourceServiceResolver.Value;

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
			var context = GetContext(HttpContext.Current);
			var resourceService = ResourceServiceResolver.Value;

			// parse the path
			var path = ParsePath(context, resourceService, fileName);

			// check if the path exists
			return resourceService.Exists(context, path);
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
			// remove the application path
			fileName = fileName.Replace(HttpContext.Current.Request.ApplicationPath, string.Empty).Trim('/', '\\');

			// create the properties
			var properties = new PropertyBag
			                 {
			                 	{"path", fileName}
			                 };

			// parse the path
			return resourceService.ParsePath(context, properties);
		}
		#endregion
		#region Mansion Accessors
		/// <summary>
		/// Gets the <see cref="IContentResourceService"/>.
		/// </summary>
		private static readonly Lazy<IApplicationResourceService> ResourceServiceResolver = new Lazy<IApplicationResourceService>(() =>
		                                                                                                                          {
		                                                                                                                          	// get the mansion application context
		                                                                                                                          	var applicationContext = MansionWebApplicationContextFactory.Instance;

		                                                                                                                          	// resolve the content resource service
		                                                                                                                          	return applicationContext.Nucleus.ResolveSingle<IApplicationResourceService>();
		                                                                                                                          });
		/// <summary>
		/// Get the <see cref="IMansionContext"/> of the current request.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext"/>.</param>
		/// <returns>Returns the <see cref="IMansionContext"/>.</returns>
		private static IMansionWebContext GetContext(HttpContext context)
		{
			// wrap the http context
			var wrappedContext = new HttpContextWrapper(context);

			// get the mansion application context
			var applicationContext = MansionWebApplicationContextFactory.Instance;

			// create the request
			var request = HttpContextAdapter.Adapt(wrappedContext);

			// get the mansion request context
			return MansionWebContext.Create(applicationContext, request);
		}
		#endregion
	}
}