using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Patterns.Prioritized;
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
			/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			protected MansionHttpHandlerBase(MansionRequestHandlerBase requestHandler)
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
				DoProcessRequest(requestContext);

				// dispose the request context
				requestContext.Dispose();
			}
			#endregion
			#region Template Methods
			/// <summary>
			/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
			/// </summary>
			/// <param name="context">An <see cref="IMansionWebContext"/> object that provides references to the intrinsic mansion objects used to service HTTP requests. </param>
			protected virtual void DoProcessRequest(IMansionWebContext context)
			{
				requestHandler.Execute(context);
			}
			#endregion
			#region Private Fields
			private readonly MansionRequestHandlerBase requestHandler;
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
			/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public ReadOnlyStateHttpHandler(MansionRequestHandlerBase requestHandler) : base(requestHandler)
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
			/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public StatefulHttpHandler(MansionRequestHandlerBase requestHandler) : base(requestHandler)
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
			/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandler"/> is null.</exception>
			public StatelesHttpHander(MansionRequestHandlerBase requestHandler) : base(requestHandler)
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

			// get the mansion request context
			var requestContext = MansionWebContext.Create(MansionWebApplicationContextFactory.Instance, wrappedContext);

			// select the handler
			var handler = HandlerList.Value.FirstOrDefault(candidate => candidate.IsSatisfiedBy(requestContext));

			// if no handler is found, it is considered an application bug
			if (handler == null)
				throw new InvalidOperationException("Could not handle request");

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
		/// <param name="requestHandler">The <see cref="MansionRequestHandlerBase"/> which does the actual work of handling the request.</param>
		protected virtual IHttpHandler DoGetHandler(IMansionWebContext context, MansionRequestHandlerBase requestHandler)
		{
			// determine if the request is stateful or not
			var requiredStateString = context.HttpContext.Request.QueryString[StateQueryStringParameterName] ?? String.Empty;

			// parse the required session state from the query srting
			var requiredByQueryString = RequiresSessionState.Parse(requiredStateString);

			// determine the highest state
			var highestStateDemanded = RequiresSessionState.DetermineHighestDemand(requiredByQueryString, requestHandler.MinimalStateDemand);

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
		private static readonly Lazy<IEnumerable<MansionRequestHandlerBase>> HandlerList = new Lazy<IEnumerable<MansionRequestHandlerBase>>(() =>
		                                                                                                                                    {
		                                                                                                                                    	// get the application context
		                                                                                                                                    	var applicationContext = MansionWebApplicationContextFactory.Instance;

		                                                                                                                                    	// resolve the MansionRequestHandlerBase implementations
		                                                                                                                                    	var handlers = applicationContext.Nucleus.Resolve<MansionRequestHandlerBase>();

		                                                                                                                                    	// return the sorted request handler array
		                                                                                                                                    	return handlers.OrderByPriority().ToArray();
		                                                                                                                                    });
		#endregion
	}
}