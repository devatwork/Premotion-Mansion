using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Diagnostics;

namespace Premotion.Mansion.Web.Hosting.AspNet.Diagnostics
{
	/// <summary>
	/// Traces the duration of a request.
	/// </summary>
	public class RequestDurationTracer : LogEntry
	{
		/// <summary>
		/// Defines the key under which the <see cref="RequestDurationTracer"/> is stored on the stack.
		/// </summary>
		public const string StackKey = "_RequestDurationLogEntry";
		/// <summary>
		/// Constructs a new request duration log entry.
		/// </summary>
		private RequestDurationTracer() : base("Premotion.Mansion.Web.Hosting.AspNet")
		{
		}
		/// <summary>
		/// Starts tracking an <see cref="RequestDurationTracer"/>.
		/// </summary>
		/// <param name="requestContext">The <see cref="IMansionWebContext"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the arguments is null.</exception>
		public static void Start(IMansionWebContext requestContext)
		{
			// validate arguments
			if (requestContext == null)
				throw new ArgumentNullException("requestContext");

			// init the values
			var entry = new RequestDurationTracer {
				RequestUrl = requestContext.Request.RequestUrl.ToString(),
				Received = DateTime.Now
			};

			// store it on the stack
			requestContext.Stack.Push(StackKey, PropertyBagAdapterFactory.Adapt(requestContext, entry), true).Dispose();
		}
		/// <summary>
		/// Writes this <see cref="RequestDurationTracer"/> to the <see cref="IMansionContext.TraceLog"/>
		/// </summary>
		/// <param name="requestContext">The <see cref="IMansionWebContext"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the arguments is null.</exception>
		public static void End(IMansionWebContext requestContext)
		{
			// validate arguments
			if (requestContext == null)
				throw new ArgumentNullException("requestContext");

			// get the entry from the stack
			IPropertyBag holder;
			if (!requestContext.Stack.TryPeek(StackKey, out holder))
				return;
			var entry = PropertyBagAdapterFactory.GetOriginalObject<RequestDurationTracer>(holder);

			// set the fields
			entry.Responded = DateTime.Now;

			// add to log
			requestContext.TraceLog.Write(entry);
		}
		/// <summary>
		/// Holds the request url.
		/// </summary>
		public string RequestUrl { get; private set; }
		/// <summary>
		/// Holds the date the request was received.
		/// </summary>
		public DateTime Received { get; private set; }
		/// <summary>
		/// Holds the date the response was send.
		/// </summary>
		public DateTime Responded { get; private set; }
		/// <summary>
		/// Gets the duration of the request.
		/// </summary>
		public TimeSpan Duration
		{
			get { return Responded - Received; }
		}
	}
}