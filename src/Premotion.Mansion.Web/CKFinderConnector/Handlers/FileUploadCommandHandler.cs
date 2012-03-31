using System;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// File upload command handler.
	/// </summary>
	public class FileUploadCommandHandler : CommandHandlerBase
	{
		#region Overrides of CommandHandlerBase
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="IMansionWebContext"/>.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected override void DoHandle(IMansionWebContext context)
		{
			// get the uploaded file
			var uploadedFile = Request.Files[Request.Files.AllKeys[0]];
			var filename = uploadedFile.FileName;
			var errorCode = ErrorCodes.None;

			// store the file
			try
			{
				//  upload the file
				var assetEntry = AssetService.StoreResource(context, CurrentAssetFolder, filename, uploadedFile.InputStream);

				// check if the file name has changed
				if (!filename.Equals(assetEntry.Name, StringComparison.OrdinalIgnoreCase))
					errorCode = ErrorCodes.UploadedFileRenamed;
			}
			catch (ConnectorException connectorException)
			{
				errorCode = connectorException.Number;
			}
			catch (SecurityException)
			{
				errorCode = ErrorCodes.AccessDenied;
			}
			catch (UnauthorizedAccessException)
			{
				errorCode = ErrorCodes.AccessDenied;
			}
			catch
			{
				errorCode = ErrorCodes.Unknown;
			}

			// send feedback
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			if ("txt".Equals(Request.QueryString["response_type"]))
			{
				Response.ContentType = "text/plaing";
				var errorMessage = string.Empty;
				if (errorCode > ErrorCodes.None)
				{
					errorMessage = "It was not possible to complete the request. (Error %1)".Replace("%1", filename);
					if (errorCode != ErrorCodes.UploadedFileRenamed && errorCode != ErrorCodes.UploadedInvalidNameRenamed)
						filename = string.Empty;
				}
				Response.Write(filename + "|" + errorMessage);
			}
			else
			{
				Response.ContentType = "text/html";
				Response.Write("<script type=\"text/javascript\">");
				Response.Write(GetJavaScriptCode(Request, errorCode, filename, CurrentAssetFolder + filename));
				Response.Write("</script>");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="errorNumber"></param>
		/// <param name="fileName"></param>
		/// <param name="fileUrl"></param>
		/// <returns></returns>
		private static string GetJavaScriptCode(HttpRequestBase request, ErrorCodes errorNumber, string fileName, string fileUrl)
		{
			var funcNum = request.QueryString["CKFinderFuncNum"];
			var errorMessage = string.Empty;

			if (errorNumber > 0)
			{
				errorMessage = "It was not possible to complete the request. (Error %1)".Replace("%1", fileName);
				if (errorNumber != ErrorCodes.UploadedFileRenamed && errorNumber != ErrorCodes.UploadedInvalidNameRenamed)
					fileName = string.Empty;
			}
			if (funcNum == null)
				return "window.parent.OnUploadCompleted('" + fileName.Replace("'", "\\'") + "','" + errorMessage.Replace("'", "\\'") + "') ;";

			funcNum = Regex.Replace(funcNum, @"[^0-9]", "", RegexOptions.None);
			if (errorNumber > 0)
			{
				errorMessage = "It was not possible to complete the request. (Error %1)".Replace("%1", fileName);
				if (errorNumber != ErrorCodes.UploadedFileRenamed)
					fileUrl = string.Empty;
			}
			return "window.parent.CKFinder.tools.callFunction(" + funcNum + ",'" + fileUrl.Replace("'", "\\'") + "','" + errorMessage.Replace("'", "\\'") + "') ;";
		}
		#endregion
	}
}