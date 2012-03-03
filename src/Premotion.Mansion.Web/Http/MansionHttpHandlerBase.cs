using System;
using System.Web;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements the basic features of an Mansion <see cref="IHttpHandler"/>.
	/// </summary>
	public abstract class MansionHttpHandlerBase : IHttpHandler
	{
		#region Implementation of IHttpHandler
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
		public void ProcessRequest(HttpContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// create the request context
			using (var mansionContext = MansionWebContext.Create(context))
				ProcessRequest(mansionContext);
		}
		/// <summary>
		/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
		/// </returns>
		public bool IsReusable
		{
			get { return true; }
		}
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/> constructed for handling the current request.</param>
		protected abstract void ProcessRequest(MansionWebContext context);
		#endregion
	}
}