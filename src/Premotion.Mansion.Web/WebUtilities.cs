using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.ScriptTags.Rendering;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Provides utility methods for web applications.
	/// </summary>
	public static class WebUtilities
	{
		#region MIME Mapping Table
		/// <summary>
		/// Maps file extensions to MIME types.
		/// </summary>
		private static readonly IDictionary<string, string> extensionToMIMEMap = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
		                                                                         {
		                                                                         	{".323", "text/h323"},
		                                                                         	{".asx", "video/x-ms-asf"},
		                                                                         	{".acx", "application/internet-property-stream"},
		                                                                         	{".ai", "application/postscript"},
		                                                                         	{".aif", "audio/x-aiff"},
		                                                                         	{".aiff", "audio/aiff"},
		                                                                         	{".axs", "application/olescript"},
		                                                                         	{".aifc", "audio/aiff"},
		                                                                         	{".asr", "video/x-ms-asf"},
		                                                                         	{".avi", "video/x-msvideo"},
		                                                                         	{".asf", "video/x-ms-asf"},
		                                                                         	{".au", "audio/basic"},
		                                                                         	{".application", "application/x-ms-application"},
		                                                                         	{".bin", "application/octet-stream"},
		                                                                         	{".bas", "text/plain"},
		                                                                         	{".bcpio", "application/x-bcpio"},
		                                                                         	{".bmp", "image/bmp"},
		                                                                         	{".cdf", "application/x-cdf"},
		                                                                         	{".cat", "application/vndms-pkiseccat"},
		                                                                         	{".crt", "application/x-x509-ca-cert"},
		                                                                         	{".c", "text/plain"},
		                                                                         	{".css", "text/css"},
		                                                                         	{".cer", "application/x-x509-ca-cert"},
		                                                                         	{".crl", "application/pkix-crl"},
		                                                                         	{".cmx", "image/x-cmx"},
		                                                                         	{".csh", "application/x-csh"},
		                                                                         	{".cod", "image/cis-cod"},
		                                                                         	{".cpio", "application/x-cpio"},
		                                                                         	{".clp", "application/x-msclip"},
		                                                                         	{".crd", "application/x-mscardfile"},
		                                                                         	{".deploy", "application/octet-stream"},
		                                                                         	{".dll", "application/x-msdownload"},
		                                                                         	{".dot", "application/msword"},
		                                                                         	{".doc", "application/msword"},
		                                                                         	{".dvi", "application/x-dvi"},
		                                                                         	{".dir", "application/x-director"},
		                                                                         	{".dxr", "application/x-director"},
		                                                                         	{".der", "application/x-x509-ca-cert"},
		                                                                         	{".dib", "image/bmp"},
		                                                                         	{".dcr", "application/x-director"},
		                                                                         	{".disco", "text/xml"},
		                                                                         	{".exe", "application/octet-stream"},
		                                                                         	{".etx", "text/x-setext"},
		                                                                         	{".evy", "application/envoy"},
		                                                                         	{".eml", "message/rfc822"},
		                                                                         	{".eps", "application/postscript"},
		                                                                         	{".flr", "x-world/x-vrml"},
		                                                                         	{".fif", "application/fractals"},
		                                                                         	{".gtar", "application/x-gtar"},
		                                                                         	{".gif", "image/gif"},
		                                                                         	{".gz", "application/x-gzip"},
		                                                                         	{".hta", "application/hta"},
		                                                                         	{".htc", "text/x-component"},
		                                                                         	{".htt", "text/webviewhtml"},
		                                                                         	{".h", "text/plain"},
		                                                                         	{".hdf", "application/x-hdf"},
		                                                                         	{".hlp", "application/winhlp"},
		                                                                         	{".html", "text/html"},
		                                                                         	{".htm", "text/html"},
		                                                                         	{".hqx", "application/mac-binhex40"},
		                                                                         	{".isp", "application/x-internet-signup"},
		                                                                         	{".iii", "application/x-iphone"},
		                                                                         	{".ief", "image/ief"},
		                                                                         	{".ivf", "video/x-ivf"},
		                                                                         	{".ins", "application/x-internet-signup"},
		                                                                         	{".ico", "image/x-icon"},
		                                                                         	{".jpg", "image/jpeg"},
		                                                                         	{".jfif", "image/pjpeg"},
		                                                                         	{".jpe", "image/jpeg"},
		                                                                         	{".jpeg", "image/jpeg"},
		                                                                         	{".js", "application/x-javascript"},
		                                                                         	{".lsx", "video/x-la-asf"},
		                                                                         	{".latex", "application/x-latex"},
		                                                                         	{".lsf", "video/x-la-asf"},
		                                                                         	{".manifest", "application/x-ms-manifest"},
		                                                                         	{".mhtml", "message/rfc822"},
		                                                                         	{".mny", "application/x-msmoney"},
		                                                                         	{".mht", "message/rfc822"},
		                                                                         	{".mid", "audio/mid"},
		                                                                         	{".mpv2", "video/mpeg"},
		                                                                         	{".man", "application/x-troff-man"},
		                                                                         	{".mvb", "application/x-msmediaview"},
		                                                                         	{".mpeg", "video/mpeg"},
		                                                                         	{".m3u", "audio/x-mpegurl"},
		                                                                         	{".mdb", "application/x-msaccess"},
		                                                                         	{".mpp", "application/vnd.ms-project"},
		                                                                         	{".m1v", "video/mpeg"},
		                                                                         	{".mpa", "video/mpeg"},
		                                                                         	{".me", "application/x-troff-me"},
		                                                                         	{".m13", "application/x-msmediaview"},
		                                                                         	{".movie", "video/x-sgi-movie"},
		                                                                         	{".m14", "application/x-msmediaview"},
		                                                                         	{".mpe", "video/mpeg"},
		                                                                         	{".mp2", "video/mpeg"},
		                                                                         	{".mov", "video/quicktime"},
		                                                                         	{".mp3", "audio/mpeg"},
		                                                                         	{".mpg", "video/mpeg"},
		                                                                         	{".ms", "application/x-troff-ms"},
		                                                                         	{".nc", "application/x-netcdf"},
		                                                                         	{".nws", "message/rfc822"},
		                                                                         	{".oda", "application/oda"},
		                                                                         	{".ods", "application/oleobject"},
		                                                                         	{".pmc", "application/x-perfmon"},
		                                                                         	{".p7r", "application/x-pkcs7-certreqresp"},
		                                                                         	{".p7b", "application/x-pkcs7-certificates"},
		                                                                         	{".p7s", "application/pkcs7-signature"},
		                                                                         	{".pmw", "application/x-perfmon"},
		                                                                         	{".png", "image/png"},
		                                                                         	{".ps", "application/postscript"},
		                                                                         	{".p7c", "application/pkcs7-mime"},
		                                                                         	{".pbm", "image/x-portable-bitmap"},
		                                                                         	{".ppm", "image/x-portable-pixmap"},
		                                                                         	{".pub", "application/x-mspublisher"},
		                                                                         	{".pnm", "image/x-portable-anymap"},
		                                                                         	{".pml", "application/x-perfmon"},
		                                                                         	{".p10", "application/pkcs10"},
		                                                                         	{".pfx", "application/x-pkcs12"},
		                                                                         	{".p12", "application/x-pkcs12"},
		                                                                         	{".pdf", "application/pdf"},
		                                                                         	{".pps", "application/vnd.ms-powerpoint"},
		                                                                         	{".p7m", "application/pkcs7-mime"},
		                                                                         	{".pko", "application/vndms-pkipko"},
		                                                                         	{".ppt", "application/vnd.ms-powerpoint"},
		                                                                         	{".pmr", "application/x-perfmon"},
		                                                                         	{".pma", "application/x-perfmon"},
		                                                                         	{".pot", "application/vnd.ms-powerpoint"},
		                                                                         	{".prf", "application/pics-rules"},
		                                                                         	{".pgm", "image/x-portable-graymap"},
		                                                                         	{".qt", "video/quicktime"},
		                                                                         	{".ra", "audio/x-pn-realaudio"},
		                                                                         	{".rgb", "image/x-rgb"},
		                                                                         	{".ram", "audio/x-pn-realaudio"},
		                                                                         	{".rmi", "audio/mid"},
		                                                                         	{".ras", "image/x-cmu-raster"},
		                                                                         	{".roff", "application/x-troff"},
		                                                                         	{".rtf", "application/rtf"},
		                                                                         	{".rtx", "text/richtext"},
		                                                                         	{".sv4crc", "application/x-sv4crc"},
		                                                                         	{".spc", "application/x-pkcs7-certificates"},
		                                                                         	{".setreg", "application/set-registration-initiation"},
		                                                                         	{".snd", "audio/basic"},
		                                                                         	{".stl", "application/vndms-pkistl"},
		                                                                         	{".setpay", "application/set-payment-initiation"},
		                                                                         	{".stm", "text/html"},
		                                                                         	{".shar", "application/x-shar"},
		                                                                         	{".sh", "application/x-sh"},
		                                                                         	{".sit", "application/x-stuffit"},
		                                                                         	{".spl", "application/futuresplash"},
		                                                                         	{".sct", "text/scriptlet"},
		                                                                         	{".scd", "application/x-msschedule"},
		                                                                         	{".sst", "application/vndms-pkicertstore"},
		                                                                         	{".src", "application/x-wais-source"},
		                                                                         	{".sv4cpio", "application/x-sv4cpio"},
		                                                                         	{".tex", "application/x-tex"},
		                                                                         	{".tgz", "application/x-compressed"},
		                                                                         	{".t", "application/x-troff"},
		                                                                         	{".tar", "application/x-tar"},
		                                                                         	{".tr", "application/x-troff"},
		                                                                         	{".tif", "image/tiff"},
		                                                                         	{".txt", "text/plain"},
		                                                                         	{".texinfo", "application/x-texinfo"},
		                                                                         	{".trm", "application/x-msterminal"},
		                                                                         	{".tiff", "image/tiff"},
		                                                                         	{".tcl", "application/x-tcl"},
		                                                                         	{".texi", "application/x-texinfo"},
		                                                                         	{".tsv", "text/tab-separated-values"},
		                                                                         	{".ustar", "application/x-ustar"},
		                                                                         	{".uls", "text/iuls"},
		                                                                         	{".vcf", "text/x-vcard"},
		                                                                         	{".wps", "application/vnd.ms-works"},
		                                                                         	{".wav", "audio/wav"},
		                                                                         	{".wrz", "x-world/x-vrml"},
		                                                                         	{".wri", "application/x-mswrite"},
		                                                                         	{".wks", "application/vnd.ms-works"},
		                                                                         	{".wmf", "application/x-msmetafile"},
		                                                                         	{".wcm", "application/vnd.ms-works"},
		                                                                         	{".wrl", "x-world/x-vrml"},
		                                                                         	{".wdb", "application/vnd.ms-works"},
		                                                                         	{".wsdl", "text/xml"},
		                                                                         	{".xml", "text/xml"},
		                                                                         	{".xlm", "application/vnd.ms-excel"},
		                                                                         	{".xaf", "x-world/x-vrml"},
		                                                                         	{".xla", "application/vnd.ms-excel"},
		                                                                         	{".xls", "application/vnd.ms-excel"},
		                                                                         	{".xof", "x-world/x-vrml"},
		                                                                         	{".xlt", "application/vnd.ms-excel"},
		                                                                         	{".xlc", "application/vnd.ms-excel"},
		                                                                         	{".xsl", "text/xml"},
		                                                                         	{".xbm", "image/x-xbitmap"},
		                                                                         	{".xlw", "application/vnd.ms-excel"},
		                                                                         	{".xpm", "image/x-xpixmap"},
		                                                                         	{".xwd", "image/x-xwindowdump"},
		                                                                         	{".xsd", "text/xml"},
		                                                                         	{".z", "application/x-compress"},
		                                                                         	{".zip", "application/x-zip-compressed"},
		                                                                         	{".woff", "application/font-woff"},
		                                                                         	{".eot", "application/vnd.ms-fontobject"},
		                                                                         	{".otf", "font/otf"},
		                                                                         	{".svg", "image/svg+xml"},
		                                                                         	{".svgz", "image/svg+xml"},
		                                                                         };
		#endregion
		#region Methods
		/// <summary>
		/// Gets the MIME type based on the file extension.
		/// </summary>
		/// <param name="filename">The file name for which to get the MIME type.</param>
		/// <returns>Returns the MIME type or 'application/octetstream' when no MIME type was found.</returns>
		public static string GetMimeType(string filename)
		{
			// validate arguments
			if (String.IsNullOrEmpty(filename))
				throw new ArgumentNullException("filename");

			// check if there is no extension
			var extension = Path.GetExtension(filename);
			if (String.IsNullOrEmpty(extension))
				throw new InvalidOperationException(String.Format("Could not find extension for filename {0}", filename));

			// try to lookup the MIME type
			string mimeType;
			return extensionToMIMEMap.TryGetValue(extension, out mimeType) ? mimeType : "application/octetstream";
		}
		/// <summary>
		/// Combines the <paramref name="parts"/> into a relative url.
		/// </summary>
		/// <param name="parts">The parts which to combine.</param>
		/// <returns>Returns the combined URL.</returns>
		public static string[] CombineIntoRelativeUrl(params string[] parts)
		{
			// validate arguments
			if (parts == null)
				throw new ArgumentNullException("parts");

			// split all the incoming parts on dash
			return parts.SelectMany(part => part.Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries)).ToArray();
		}
		#endregion
		#region Cache Control Methods
		/// <summary>
		/// Disables the output cache of the current request.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public static void DisableOutputCache(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the output pipe
			var webOutputPipe = context.GetWebOuputPipe();

			// disable the cache
			webOutputPipe.Response.CacheSettings.OutputCacheEnabled = false;
		}
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		public static void DisableResponseTemplateCache(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// find all the response template caches and disable their caches
			foreach (var responsePipe in context.OutputPipeStack.OfType<ResponseTemplateTag.ResponseOutputPipe>())
				responsePipe.ResponseCacheEnabled = false;
		}
		#endregion
		#region RedirectRequest Methods
		/// <summary>
		/// Redirects the request to the target <paramref name="url"/>. Disables the response and output caches and halts the script execution.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="url">The <see cref="Url"/> to which to redirect.</param>
		/// <param name="permanent">Flag indicating whether the redirect is permanent or not.</param>
		public static void RedirectRequest(IMansionContext context, Url url, bool permanent = false)
		{
			// get the output pipe
			var webOutputPipe = context.GetWebOuputPipe();

			// disable the caches
			DisableOutputCache(context);
			DisableResponseTemplateCache(context);

			// set redirect
			webOutputPipe.Response.RedirectLocation = url;
			webOutputPipe.Response.StatusCode = permanent ? HttpStatusCode.MovedPermanently : HttpStatusCode.Found;

			// halt execution
			context.BreakExecution = true;
		}
		#endregion
	}
}