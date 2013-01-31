using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Premotion.Mansion.Web.Hosting.AspNet
{
	/// <summary>
	/// Adapts the given <see cref="HttpRequestBase"/> to <see cref="WebRequest"/>.
	/// </summary>
	public static class HttpContextAdapter
	{
		#region Adapt Methods
		/// <summary>
		/// Adapts the <see cref="HttpContextBase"/> to a <see cref="WebRequest"/>.
		/// </summary>
		/// <param name="httpContext">The <see cref="HttpRequestBase"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static WebRequest Adapt(HttpContextBase httpContext)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			// get the variables
			var httpRequest = httpContext.Request;

			// build the application uri
			if (httpRequest.Url == null)
				throw new InvalidOperationException("The request url should not be null");
			var applicationUriBuilder = new UriBuilder {
				Scheme = httpRequest.Url.Scheme,
				Host = httpRequest.Url.Host,
				Port = httpRequest.Url.Port,
				Path = httpRequest.ApplicationPath ?? string.Empty
			};

			// create a new request
			var request = new WebRequest(httpContext.Items) {
				Body = httpRequest.InputStream,
				Method = httpRequest.HttpMethod,
				UserAgent = httpRequest.UserAgent,
				ApplicationUrl = Url.ParseApplicationUri(applicationUriBuilder.Uri)
			};

			// parse url
			request.RequestUrl = Url.ParseUri(request.ApplicationUrl, httpRequest.Url);

			// map the cookies
			foreach (var entry in httpRequest.Cookies.Cast<string>().Select(key => new KeyValuePair<string, HttpCookie>(key, httpRequest.Cookies[key])))
			{
				request.Cookies.Add(entry.Key, new WebCookie {
					Domain = entry.Value.Domain,
					Expires = entry.Value.Expires,
					HttpOnly = entry.Value.HttpOnly,
					Name = entry.Value.Name,
					Secure = entry.Value.Secure,
					Value = entry.Value.Value
				});
			}

			// map the files
			foreach (var entry in httpRequest.Files.Cast<string>().Select(key => new KeyValuePair<string, HttpPostedFileBase>(key, httpRequest.Files[key])))
			{
				request.Files.Add(entry.Key, new WebFile {
					ContentLength = entry.Value.ContentLength,
					ContentType = entry.Value.ContentType,
					FileName = entry.Value.FileName,
					InputStream = entry.Value.InputStream
				});
			}

			// map the form variables
			foreach (var entry in httpRequest.Form.Cast<string>().Select(key => new KeyValuePair<string, string>(key, httpRequest.Form[key])))
				request.Form.Add(entry);

			// map the headers
			foreach (var entry in httpRequest.Headers.Cast<string>().Select(key => new KeyValuePair<string, string>(key, httpRequest.Headers[key])))
				request.Headers.Add(entry);

			return request;
		}
		#endregion
		#region Transfer Methods
		/// <summary>
		/// Transfers the <paramref name="response"/> data to <paramref name="httpResponse"/>.
		/// </summary>
		/// <param name="response">The <see cref="WebResponse"/>.</param>
		/// <param name="httpResponse">The <see cref="HttpResponseBase"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static void Transfer(WebResponse response, HttpResponseBase httpResponse)
		{
			// validate arguments
			if (response == null)
				throw new ArgumentNullException("response");
			if (httpResponse == null)
				throw new ArgumentNullException("httpResponse");

			// map the response to the http output
			httpResponse.ContentEncoding = response.ContentEncoding;
			httpResponse.ContentType = response.ContentType;
			httpResponse.StatusCode = (int) response.StatusCode;
			httpResponse.StatusDescription = response.StatusDescription;

			// flush the content
			response.Contents(httpResponse.OutputStream);

			// copy headers
			foreach (var header in response.Headers)
				httpResponse.AddHeader(header.Key, header.Value);

			// transfer all the cookies to the http response
			TransferCookies(response, httpResponse);

			// set cache properties
			if (response.CacheSettings.OutputCacheEnabled)
			{
				httpResponse.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
				if (response.CacheSettings.Expires.HasValue)
					httpResponse.Cache.SetExpires(response.CacheSettings.Expires.Value);
				else if (!string.IsNullOrEmpty(response.CacheSettings.ETag))
				{
					httpResponse.Cache.SetLastModified(response.CacheSettings.LastModified);
					httpResponse.Cache.SetETag(response.CacheSettings.ETag);
				}
			}

			// check for redirect
			if (!string.IsNullOrEmpty(response.RedirectLocation))
				httpResponse.RedirectLocation = response.RedirectLocation;
		}
		/// <summary>
		/// Transfers all the <see cref="WebResponse.Cookies"/> to  <see cref="HttpResponseBase.Cookies"/>.
		/// </summary>
		/// <param name="response">The <see cref="WebResponse"/>.</param>
		/// <param name="httpResponse">The <see cref="HttpResponse"/>.</param>
		private static void TransferCookies(WebResponse response, HttpResponseBase httpResponse)
		{
			// copy cookies
			foreach (var cookie in response.Cookies)
				TransferCookie(cookie, httpResponse);
		}
		/// <summary>
		/// Transfers the given <paramref name="cookie"/> to the <paramref name="httpResponse"/>.
		/// </summary>
		/// <param name="cookie">The <see cref="WebCookie"/>.</param>
		/// <param name="httpResponse">The <see cref="HttpResponseBase"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static void TransferCookie(WebCookie cookie, HttpResponseBase httpResponse)
		{
			// validate arguments
			if (cookie == null)
				throw new ArgumentNullException("cookie");
			if (httpResponse == null)
				throw new ArgumentNullException("httpResponse");

			// create the http cookie
			var httpCookie = new HttpCookie(cookie.Name, cookie.Value) {
				Secure = cookie.Secure,
				HttpOnly = cookie.HttpOnly
			};

			// check for domain
			if (!string.IsNullOrEmpty(cookie.Domain))
				httpCookie.Domain = cookie.Domain;

			// check expires
			if (cookie.Expires.HasValue)
				httpCookie.Expires = cookie.Expires.Value;

			// add the cookie to the response
			httpResponse.Cookies.Add(httpCookie);
		}
		#endregion
	}
}