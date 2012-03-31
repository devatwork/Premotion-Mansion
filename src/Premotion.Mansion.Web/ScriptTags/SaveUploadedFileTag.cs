using System;
using System.IO;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Saves an uploaded file.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "saveUploadedFile")]
	public class SaveUploadedFileTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentResourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public SaveUploadedFileTag(IContentResourceService contentResourceService)
		{
			// validate arguments
			if (contentResourceService == null)
				throw new ArgumentNullException("contentResourceService");

			// set values
			this.contentResourceService = contentResourceService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the web context
			var webContext = context.Cast<IMansionWebContext>();

			// check if there is exactly one uploaded file
			if (webContext.HttpContext.Request.Files.Count != 1)
				throw new InvalidOperationException(string.Format("There were {0} uploaded files instead of 1", webContext.HttpContext.Request.Files.Count));

			// get the uploaded file
			var uploadedFile = webContext.HttpContext.Request.Files[0];

			// store the file
			var resourcePath = contentResourceService.ParsePath(context, new PropertyBag
			                                                             {
			                                                             	{"fileName", uploadedFile.FileName},
			                                                             	{"category", GetAttribute(context, "category", "Uploads")}
			                                                             });
			var resource = contentResourceService.GetResource(context, resourcePath);
			using (var pipe = resource.OpenForWriting())
				uploadedFile.InputStream.CopyTo(pipe.RawStream);

			// set the path to the file as the value of the property
			var uploadedFilePath = contentResourceService.GetFirstRelativePath(context, resourcePath);

			// create the properties
			var uploadedFileProperties = new PropertyBag
			                             {
			                             	{"fileName", Path.GetFileName(uploadedFilePath)},
			                             	{"relativePath", uploadedFilePath}
			                             };
			using (context.Stack.Push(GetRequiredAttribute<string>(context, "target"), uploadedFileProperties, GetAttribute(context, "global", false)))
				ExecuteChildTags(context);
		}
		#endregion
		#region Private Fields
		private readonly IContentResourceService contentResourceService;
		#endregion
	}
}