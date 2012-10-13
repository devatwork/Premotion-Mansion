using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Web.Hosting.AspNet
{
	/// <summary>
	/// Implements the request routing for all mansion requests.
	/// </summary>
	public class MansionHttpHandlerFactory : IHttpHandlerFactory
	{
		#region Constants
		/// <summary>
		/// Identifies the query string parameter used to determine the session state type in which to serve the current request.
		/// </summary>
		public const string StateQueryStringParameterName = "session-state";
		#endregion
		#region Nested type: HttpHandlerBase
		/// <summary>
		/// Abstract implementation of the <see cref="IHttpHandler"/>.
		/// </summary>
		protected abstract class HttpHandlerBase : IHttpHandler
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

				// wrap the http context
				var wrappedContext = new HttpContextWrapper(context);

				// invoke template method
				DoProcessRequest(wrappedContext);
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
			#endregion
			#region Template Methods
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="context">An <see cref="T:System.Web.HttpContextBase"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
			protected abstract void DoProcessRequest(HttpContextBase context);
			#endregion
		}
		#endregion
		#region Nested type: MansionHttpHandlerBase
		/// <summary>
		/// Base type for handlers which execute requests in a <see cref="IMansionWebContext"/>.
		/// </summary>
		protected abstract class MansionHttpHandlerBase : HttpHandlerBase
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			protected MansionHttpHandlerBase(RequestHandler requestHandler)
			{
				// validate arguments
				if (requestHandler == null)
					throw new ArgumentNullException("requestHandler");

				// set values
				this.requestHandler = requestHandler;
			}
			#endregion
			#region Overrides of HttpHandlerBase
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="context">An <see cref="T:System.Web.HttpContextBase"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
			protected override sealed void DoProcessRequest(HttpContextBase context)
			{
				// get the request context
				var requestContext = MansionWebContext.FetchFromHttpContext(context) as MansionWebContext;

				// initialize the the security service when there is state
				if (requestContext.HttpContext.HasSession())
					requestContext.Nucleus.ResolveSingle<ISecurityService>().InitializeSecurityContext(requestContext);

				// create the request context
				DoProcessRequest(context, requestContext);

				// dispose the request context
				requestContext.Dispose();
			}
			#endregion
			#region Template Methods
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="context">An <see cref="IMansionWebContext"/> object that provides references to the intrinsic mansion objects used to service HTTP requests. </param>
			/// <param name="requestContext">The <see cref="IMansionWebContext"/>.</param>
			protected virtual void DoProcessRequest(HttpContextBase context, IMansionWebContext requestContext)
			{
				// get the response
				var webResponse = requestHandler.Execute(requestContext);

				// map the response to the http output
				var httpResponse = context.Response;
				httpResponse.ContentEncoding = webResponse.ContentEncoding;
				httpResponse.ContentType = webResponse.ContentType;
				httpResponse.StatusCode = (int) webResponse.StatusCode;
				httpResponse.StatusDescription = webResponse.StatusDescription;

				// flush the content
				webResponse.Contents(httpResponse.OutputStream);

				// copy headers
				foreach (var header in webResponse.Headers)
					httpResponse.AddHeader(header.Key, header.Value);

				// copy cookies
				foreach (var cookie in webResponse.Cookies)
				{
					// create the http cookie
					var httpCookie = new HttpCookie(cookie.Name, cookie.Value)
					                 {
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

				// set cache properties
				if (webResponse.CacheSettings.OutputCacheEnabled)
				{
					if (webResponse.CacheSettings.Expires.HasValue)
						httpResponse.Cache.SetExpires(webResponse.CacheSettings.Expires.Value);
					else if (!string.IsNullOrEmpty(webResponse.CacheSettings.ETag))
					{
						httpResponse.Cache.SetLastModified(webResponse.CacheSettings.LastModified);
						httpResponse.Cache.SetETag(webResponse.CacheSettings.ETag);
					}
				}

				// check for redirect
				if (!string.IsNullOrEmpty(webResponse.RedirectLocation))
					httpResponse.RedirectLocation = webResponse.RedirectLocation;
			}
			#endregion
			#region Private Fields
			private readonly RequestHandler requestHandler;
			#endregion
		}
		#endregion
		#region Nested type: ReadOnlyStateHttpHandler
		/// <summary>
		/// Implements <see cref="MansionHttpHandlerBase"/> with read-only session state. This prevents locking on the session object and allows IIS to serve several requests simultaneously for the same session.
		/// </summary>
		protected class ReadOnlyStateHttpHandler : MansionHttpHandlerBase, IReadOnlySessionState
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public ReadOnlyStateHttpHandler(RequestHandler requestHandler) : base(requestHandler)
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: StatefulHttpHandler
		/// <summary>
		/// Implements <see cref="MansionHttpHandlerBase"/> with session state. Requests originating from the same session can not be handled simultaneously by IIS.
		/// </summary>
		protected class StatefulHttpHandler : MansionHttpHandlerBase, IRequiresSessionState
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public StatefulHttpHandler(RequestHandler requestHandler) : base(requestHandler)
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: StatelesHttpHander
		/// <summary>
		/// Implements <see cref="MansionHttpHandlerBase"/> without session state. Requests originating from the same session can always be handled simultaneously by IIS.
		/// </summary>
		protected class StatelesHttpHander : MansionHttpHandlerBase
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public StatelesHttpHander(RequestHandler requestHandler) : base(requestHandler)
			{
			}
			#endregion
		}
		#endregion
		#region Implementation of IHttpHandlerFactory
		/// <summary>
		/// Returns an instance of a class that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Web.IHttpHandler"/> object that processes the request.
		/// </returns>
		/// <param name="context">An instance of the <see cref="T:System.Web.HttpContext"/> class that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param><param name="requestType">The HTTP data transfer method (GET or POST) that the client uses. </param><param name="url">The <see cref="P:System.Web.HttpRequest.RawUrl"/> of the requested resource. </param><param name="pathTranslated">The <see cref="P:System.Web.HttpRequest.PhysicalApplicationPath"/> to the requested resource. </param>
		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (requestType == null)
				throw new ArgumentNullException("requestType");
			if (url == null)
				throw new ArgumentNullException("url");
			if (pathTranslated == null)
				throw new ArgumentNullException("pathTranslated");

			// wrap the http context
			var wrappedContext = new HttpContextWrapper(context);

			// get the mansion application context
			var applicationContext = MansionWebApplicationContextFactory.Instance;

			// get the mansion request context
			var requestContext = MansionWebContext.Create(applicationContext, wrappedContext);

			// select the handler
			RequestHandlerFactory handlerFactory;
			if (!Election<RequestHandlerFactory, IMansionWebContext>.TryElect(applicationContext, HandlerFactories.Value, requestContext, out handlerFactory))
				return null;

			// create the handler
			var handler = handlerFactory.Create(applicationContext);

			// allow the configurators to configure the request handler
			foreach (var configurator in HandlerConfigurators.Value)
				configurator.Configure(requestContext, handler);

			// select the proper handler type
			return DoGetHandler(requestContext, handler);
		}
		/// <summary>
		/// Enables a factory to reuse an existing handler instance.
		/// </summary>
		/// <param name="handler">The <see cref="T:System.Web.IHttpHandler"/> object to reuse. </param>
		public void ReleaseHandler(IHttpHandler handler)
		{
			// do nothing
		}
		/// <summary>
		/// Returns an instance of a class that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Web.IHttpHandler"/> object that processes the request.
		/// </returns>
		/// <param name="context">An instance of the <see cref="T:System.Web.HttpContextBase"/> class that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
		/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
		protected virtual IHttpHandler DoGetHandler(IMansionWebContext context, RequestHandler requestHandler)
		{
			// determine if the request is stateful or not
			var requiredStateString = context.HttpContext.Request.QueryString[StateQueryStringParameterName] ?? String.Empty;

			// parse the required session state from the query srting
			var requiredByQueryString = RequiresSessionState.Parse(requiredStateString);

			// determine the highest state
			var highestStateDemanded = RequiresSessionState.DetermineHighestDemand(requiredByQueryString, requestHandler.MinimalSessionStateDemand);

			// switch to the corrent http handler
			if (highestStateDemanded == RequiresSessionState.Full)
				return new StatefulHttpHandler(requestHandler);
			if (highestStateDemanded == RequiresSessionState.ReadOnly)
				return new ReadOnlyStateHttpHandler(requestHandler);
			if (highestStateDemanded == RequiresSessionState.No)
				return new StatelesHttpHander(requestHandler);
			throw new InvalidOperationException("Uknown session state demanded");
		}
		#endregion
		#region Private Fields
		private static readonly Lazy<IEnumerable<RequestHandlerFactory>> HandlerFactories = new Lazy<IEnumerable<RequestHandlerFactory>>(() =>
		                                                                                                                                 {
		                                                                                                                                 	// get the application context
		                                                                                                                                 	var applicationContext = MansionWebApplicationContextFactory.Instance;

		                                                                                                                                 	// resolve the RequestHandler implementations
		                                                                                                                                 	var factories = applicationContext.Nucleus.Resolve<RequestHandlerFactory>();

		                                                                                                                                 	// return the sorted request handler array
		                                                                                                                                 	return factories.ToArray();
		                                                                                                                                 });
		private static readonly Lazy<IEnumerable<RequestHandlerConfigurator>> HandlerConfigurators = new Lazy<IEnumerable<RequestHandlerConfigurator>>(() =>
		                                                                                                                                               {
		                                                                                                                                               	// get the application context
		                                                                                                                                               	var applicationContext = MansionWebApplicationContextFactory.Instance;

		                                                                                                                                               	// resolve the RequestHandler implementations
		                                                                                                                                               	var configurators = applicationContext.Nucleus.Resolve<RequestHandlerConfigurator>();

		                                                                                                                                               	// return the sorted request handler array
		                                                                                                                                               	return configurators.ToArray();
		                                                                                                                                               });
		#endregion
	}
}