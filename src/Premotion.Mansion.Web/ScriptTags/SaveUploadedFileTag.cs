using System;
using System.IO;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Saves an uploaded file.
	/// </summary>
	[Named(Constants.NamespaceUri, "saveUploadedFile")]
	public class SaveUploadedFileTag : ScriptTag
	{
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			// get the web context
			var webContext = context.Cast<MansionWebContext>();

			// check if there is exactly one uploaded file
			if (webContext.HttpContext.Request.Files.Count != 1)
				throw new InvalidOperationException(string.Format("There were {0} uploaded files instead of 1", webContext.HttpContext.Request.Files.Count));

			// get the uploaded file
			var uploadedFile = webContext.HttpContext.Request.Files[0];

			// store the file
			var contentResourceService = context.Nucleus.Get<IContentResourceService>(context);
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
	}
}