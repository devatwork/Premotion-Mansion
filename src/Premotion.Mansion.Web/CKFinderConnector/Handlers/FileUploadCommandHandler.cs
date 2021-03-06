﻿using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

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
			var uploadedFile = Request.Files.Values.First();
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
			Response.CacheSettings.OutputCacheEnabled = false;
			if ("txt".Equals(Request.RequestUrl.QueryString["response_type"]))
			{
				Response.ContentType = "text/plain";
				var errorMessage = string.Empty;
				if (errorCode > ErrorCodes.None)
				{
					errorMessage = "It was not possible to complete the request. (Error %1)".Replace("%1", filename);
					if (errorCode != ErrorCodes.UploadedFileRenamed && errorCode != ErrorCodes.UploadedInvalidNameRenamed)
						filename = string.Empty;
				}
				Response.Contents = stream =>
				                    {
				                    	using (var writer = new StreamWriter(stream, Response.ContentEncoding))
				                    		writer.Write(filename + "|" + errorMessage);
				                    };
			}
			else
			{
				Response.ContentType = "text/html";
				Response.Contents = stream =>
				                    {
				                    	using (var writer = new StreamWriter(stream, Response.ContentEncoding))
				                    	{
				                    		writer.Write("<script type=\"text/javascript\">");
				                    		writer.Write(GetJavaScriptCode(Request, errorCode, filename, CurrentAssetFolder + filename));
				                    		writer.Write("</script>");
				                    	}
				                    };
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
		private static string GetJavaScriptCode(WebRequest request, ErrorCodes errorNumber, string fileName, string fileUrl)
		{
			var funcNum = request.RequestUrl.QueryString["CKFinderFuncNum"];
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