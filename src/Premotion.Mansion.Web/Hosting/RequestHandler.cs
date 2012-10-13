using System;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Base class for all the request handlers.
	/// </summary>
	public abstract class RequestHandler
	{
		#region Execute Methods
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <returns>Returns the <see cref="WebResponse"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public WebResponse Execute(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the response from the before pipeline, or this request handler
			var response = BeforePipeline.Execute(context) ?? DoExecute(context);

			// finally execute the after pipeline
			AfterPipeline.Execute(context, response);

			// return the response
			return response;
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <returns>Returns the <see cref="WebResponse"/>.</returns>
		protected abstract WebResponse DoExecute(IMansionWebContext context);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="BeforePipeline"/> of this request handler.
		/// </summary>
		public BeforePipeline BeforePipeline
		{
			get { return beforePipeline; }
		}
		/// <summary>
		/// Gets the <see cref="BeforePipeline"/> of this request handler.
		/// </summary>
		public AfterPipeline AfterPipeline
		{
			get { return afterPipeline; }
		}
		/// <summary>
		/// Gets the minimal required <see cref="RequiresSessionState"/> for this handler.
		/// </summary>
		public virtual RequiresSessionState MinimalSessionStateDemand
		{
			get { return RequiresSessionState.No; }
		}
		#endregion
		#region Private Fields
		private readonly AfterPipeline afterPipeline = new AfterPipeline();
		private readonly BeforePipeline beforePipeline = new BeforePipeline();
		#endregion
	}
}