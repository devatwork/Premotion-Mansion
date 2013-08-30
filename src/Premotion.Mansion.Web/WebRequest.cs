using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents a web request.
	/// </summary>
	public class WebRequest : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// Constructs a request object.
		/// </summary>
		/// <param name="cache">The request specific cache.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="cache"/> is null.</exception>
		public WebRequest(IDictionary cache)
		{
			// validate arguments
			if (cache == null)
				throw new ArgumentNullException("cache");

			// set values
			Cookies = new Dictionary<string, WebCookie>(StringComparer.OrdinalIgnoreCase);
			Files = new Dictionary<string, WebFile>(StringComparer.OrdinalIgnoreCase);
			Form = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Cache = cache;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the cookie collection.
		/// </summary>
		public IDictionary<string, WebCookie> Cookies { get; private set; }
		/// <summary>
		/// Gets a <see cref="Stream"/> that can be used to read the incoming HTTP body
		/// </summary>
		/// <value>A <see cref="Stream"/> object representing the incoming HTTP body.</value>
		public Stream Body { get; set; }
		/// <summary>
		/// Gets a collection of files sent by the client-
		/// </summary>
		/// <value>An <see cref="IEnumerable{T}"/> instance, containing an <see cref="WebFile"/> instance for each uploaded file.</value>
		public IDictionary<string, WebFile> Files { get; private set; }
		/// <summary>
		/// Gets the form data of this request.
		/// </summary>
		public IDictionary<string, string> Form { get; private set; }
		/// <summary>
		/// Gets the headers.
		/// </summary>
		public IDictionary<string, string> Headers { get; private set; }
		/// <summary>
		/// Gets the HTTP data transfer method used by the client.
		/// </summary>
		/// <value>The method.</value>
		public string Method { get; set; }
		/// <summary>
		/// Gets the <see cref="Url"/> application url.
		/// </summary>
		public Url ApplicationUrl { get; set; }
		/// <summary>
		/// Gets the referrer <see cref="Url"/> url if there is any.
		/// </summary>
		public Url ReferrerUrl { get; set; }
		/// <summary>
		/// Gets the <see cref="Url"/> of the request.
		/// </summary>
		public Url RequestUrl { get; set; }
		/// <summary>
		/// Gets the user agent.
		/// </summary>
		public string UserAgent { get; set; }
		/// <summary>
		/// Gets a request specific cache.
		/// </summary>
		public IDictionary Cache { get; private set; }
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			// check for managed disposal
			if (!disposeManagedResources)
				return;

			//check if the body stream can be disposed
			if (Body != null)
				Body.Dispose();

			// dispose all the files
			foreach (var file in Files.Values)
				file.Dispose();
		}
		#endregion
	}
}