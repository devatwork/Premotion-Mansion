using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Web.Hosting.AspNet.Diagnostics;

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
		protected abstract class HttpHandlerBase : DisposableBase, IHttpHandler
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
			/// <param name="requestContext">The <see cref="MansionWebContext"/> for this request.</param>
			/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			protected MansionHttpHandlerBase(MansionWebContext requestContext, RequestHandler requestHandler)
			{
				// validate arguments
				if (requestContext == null)
					throw new ArgumentNullException("requestContext");
				if (requestHandler == null)
					throw new ArgumentNullException("requestHandler");

				// set values
				webRequestContext = requestContext;
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
				// set the session
				webRequestContext.Session = GetSession(context);

				// initialize the security service
				webRequestContext.Nucleus.ResolveSingle<ISecurityService>().InitializeSecurityContext(webRequestContext);

				// create the request context
				var response = DoProcessRequest(webRequestContext);

				// transfer the response
				HttpContextAdapter.Transfer(response, context.Response);

				// end the request trace
				RequestDurationTracer.End(webRequestContext);
			}
			#endregion
			#region Template Methods
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="requestContext">The <see cref="IMansionWebContext"/>.</param>
			protected virtual WebResponse DoProcessRequest(IMansionWebContext requestContext)
			{
				// get the response
				return requestHandler.Execute(requestContext);
			}
			/// <summary>
			/// Gets the <see cref="ISession"/>.
			/// </summary>
			/// <param name="context">The <see cref="HttpContextBase"/>.</param>
			/// <returns>Returns the <see cref="ISession"/>.</returns>
			protected abstract ISession GetSession(HttpContextBase context);
			#endregion
			#region Overrides of DisposableBase
			/// <summary>
			/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
			/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
			/// </summary>
			/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
			protected override void DisposeResources(bool disposeManagedResources)
			{
				if (!disposeManagedResources)
					return;

				// dispose
				webRequestContext.Dispose();
			}
			#endregion
			#region Private Fields
			private readonly RequestHandler requestHandler;
			private readonly MansionWebContext webRequestContext;
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
			/// <param name="requestContext">The <see cref="MansionWebContext"/> for this request.</param>
			/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public ReadOnlyStateHttpHandler(MansionWebContext requestContext, RequestHandler requestHandler) : base(requestContext, requestHandler)
			{
			}
			#endregion
			#region Overrides of MansionHttpHandlerBase
			/// <summary>
			/// Gets the <see cref="ISession"/>.
			/// </summary>
			/// <param name="context">The <see cref="HttpContextBase"/>.</param>
			/// <returns>Returns the <see cref="ISession"/>.</returns>
			protected override ISession GetSession(HttpContextBase context)
			{
				return new AspNetReadOnlySession(context.Session);
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
			/// <param name="requestContext">The <see cref="MansionWebContext"/> for this request.</param>
			/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public StatefulHttpHandler(MansionWebContext requestContext, RequestHandler requestHandler) : base(requestContext, requestHandler)
			{
			}
			#endregion
			#region Overrides of MansionHttpHandlerBase
			/// <summary>
			/// Gets the <see cref="ISession"/>.
			/// </summary>
			/// <param name="context">The <see cref="HttpContextBase"/>.</param>
			/// <returns>Returns the <see cref="ISession"/>.</returns>
			protected override ISession GetSession(HttpContextBase context)
			{
				return new AspNetSession(context.Session);
			}
			#endregion
		}
		#endregion
		#region Nested type: StatelessHttpHander
		/// <summary>
		/// Implements <see cref="MansionHttpHandlerBase"/> without session state. Requests originating from the same session can always be handled simultaneously by IIS.
		/// </summary>
		protected class StatelessHttpHander : MansionHttpHandlerBase
		{
			#region Constructors
			/// <summary>
			/// Constructs this <see cref="MansionHttpHandlerBase"/> with the given <paramref name="requestHandler"/>.
			/// </summary>
			/// <param name="requestContext">The <see cref="MansionWebContext"/> for this request.</param>
			/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public StatelessHttpHander(MansionWebContext requestContext, RequestHandler requestHandler) : base(requestContext, requestHandler)
			{
			}
			#endregion
			#region Overrides of MansionHttpHandlerBase
			/// <summary>
			/// Gets the <see cref="ISession"/>.
			/// </summary>
			/// <param name="context">The <see cref="HttpContextBase"/>.</param>
			/// <returns>Returns the <see cref="ISession"/>.</returns>
			protected override ISession GetSession(HttpContextBase context)
			{
				return new NoSession();
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

			// create the request
			var request = HttpContextAdapter.Adapt(wrappedContext);

			// get the mansion request context
			var requestContext = MansionWebContext.Create(applicationContext, request);

			// start a request trace
			RequestDurationTracer.Start(requestContext);

			// select the handler
			RequestHandlerFactory handlerFactory;
			if (!Election<RequestHandlerFactory, IMansionWebContext>.TryElect(applicationContext, HandlerFactories.Value, requestContext, out handlerFactory))
			{
				// check for integrated pipeline
				if (!HttpRuntime.UsingIntegratedPipeline)
					throw new NotSupportedException("Can not do 404 redirect on non-integrated pipeline applications");

				// add new headers
				wrappedContext.Request.Headers.Add(Dispatcher.Constants.ForwardedFrom404HeaderKey, request.RequestUrl);

				// transfer the request
				wrappedContext.Server.TransferRequest("~/Default.xts", true, wrappedContext.Request.HttpMethod, wrappedContext.Request.Headers);
				wrappedContext.ApplicationInstance.CompleteRequest();

				// do not return a handler
				return null;
			}

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
			// dispose the handler
			var disposableHandler = handler as IDisposable;
			if (disposableHandler != null)
				disposableHandler.Dispose();
		}
		/// <summary>
		/// Returns an instance of a class that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Web.IHttpHandler"/> object that processes the request.
		/// </returns>
		/// <param name="context">An instance of the <see cref="T:System.Web.HttpContextBase"/> class that provides references to intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
		/// <param name="requestHandler">The <see cref="RequestHandler"/> which does the actual work of handling the request.</param>
		protected virtual IHttpHandler DoGetHandler(MansionWebContext context, RequestHandler requestHandler)
		{
			// determine if the request is stateful or not
			string requiredStateString;
			if (!context.Request.RequestUrl.QueryString.TryGetValue(StateQueryStringParameterName, out requiredStateString))
				requiredStateString = string.Empty;

			// parse the required session state from the query srting
			var requiredByQueryString = RequiresSessionState.Parse(requiredStateString);

			// determine the highest state
			var highestStateDemanded = RequiresSessionState.DetermineHighestDemand(requiredByQueryString, requestHandler.MinimalSessionStateDemand);

			// switch to the corrent http handler
			if (highestStateDemanded == RequiresSessionState.Full)
				return new StatefulHttpHandler(context, requestHandler);
			if (highestStateDemanded == RequiresSessionState.ReadOnly)
				return new ReadOnlyStateHttpHandler(context, requestHandler);
			if (highestStateDemanded == RequiresSessionState.No)
				return new StatelessHttpHander(context, requestHandler);
			throw new InvalidOperationException("Uknown session state demanded");
		}
		#endregion
		#region Private Fields
		private static readonly Lazy<IEnumerable<RequestHandlerFactory>> HandlerFactories = new Lazy<IEnumerable<RequestHandlerFactory>>(() => {
			// get the application context
			var applicationContext = MansionWebApplicationContextFactory.Instance;

			// resolve the RequestHandler implementations
			var factories = applicationContext.Nucleus.Resolve<RequestHandlerFactory>();

			// return the sorted request handler array
			return factories.ToArray();
		});
		private static readonly Lazy<IEnumerable<RequestHandlerConfigurator>> HandlerConfigurators = new Lazy<IEnumerable<RequestHandlerConfigurator>>(() => {
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