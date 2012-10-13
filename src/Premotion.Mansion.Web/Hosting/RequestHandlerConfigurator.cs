using System;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Configures <see cref="RequestHandler"/>s before they handle the request.
	/// 
	/// This class should be used to configure the before and after pipelines.
	/// </summary>
	[Exported(typeof (RequestHandlerConfigurator))]
	public abstract class RequestHandlerConfigurator
	{
		#region Configure Methods
		/// <summary>
		/// Allows the configuration of the given <paramref name="handler"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="handler">The <see cref="RequestHandler"/> selected to handle the request.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Configure(IMansionWebContext context, RequestHandler handler)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (handler == null)
				throw new ArgumentNullException("handler");

			// invoke template method
			DoConfigure(context, handler);
		}
		/// <summary>
		/// Allows the configuration of the given <paramref name="handler"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="handler">The <see cref="RequestHandler"/> selected to handle the request.</param>
		protected abstract void DoConfigure(IMansionWebContext context, RequestHandler handler);
		#endregion
	}
	/// <summary>
	/// Configures <see cref="RequestHandler"/>s before they handle the request.
	/// 
	/// This class should be used to configure the before and after pipelines.
	/// </summary>
	/// <typeparam name="TRequestHandler">The type of <see cref="RequestHandler"/> configured by this handler.</typeparam>
	public abstract class RequestHandlerConfigurator<TRequestHandler> : RequestHandlerConfigurator where TRequestHandler : RequestHandler
	{
		#region Overrides of RequestHandlerConfigurator
		/// <summary>
		/// Allows the configuration of the given <paramref name="handler"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="handler">The <see cref="RequestHandler"/> selected to handle the request.</param>
		protected override void DoConfigure(IMansionWebContext context, RequestHandler handler)
		{
			// cast the handler to the determined type
			var typedHandler = handler as TRequestHandler;
			if (typedHandler == null)
				return;

			// invoke template handler
			DoConfigure(context, typedHandler);
		}
		/// <summary>
		/// Allows the configuration of the given <paramref name="handler"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="handler">The <see cref="RequestHandler"/> selected to handle the request.</param>
		protected abstract void DoConfigure(IMansionWebContext context, TRequestHandler handler);
		#endregion
	}
}