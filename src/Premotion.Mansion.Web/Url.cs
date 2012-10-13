using System;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents an URL.
	/// </summary>
	public class Url
	{
		/// <summary>
		/// Gets the path of the request, relative to the base path.
		/// 
		/// This property drives the route matching
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// Gets the query string of this url.
		/// </summary>
		public IPropertyBag QueryString { get; private set; }
		/// <summary>
		/// Gets the segments of the path.
		/// </summary>
		public string[] PathSegments { get; set; }
		/// <summary>
		/// Gets the host name
		/// </summary>
		public string HostName { get; set; }
		/// <summary>
		/// Gets/Sets the fragment of this url.
		/// </summary>
		public string Fragment { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static implicit operator string(Url url)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Url Clone()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="webContext"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static Url CreateUrl(IMansionWebContext webContext)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="webContext"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static Url CreateUrl(IMansionWebContext webContext, Url url)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="webContext"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static Url CreateUrl(IMansionWebContext webContext, Uri uri)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="webContext"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Url MakeRelative(IMansionWebContext webContext)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="webContext"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Url MakeAbsolute(IMansionWebContext webContext)
		{
			throw new NotImplementedException();
		}
	}
}