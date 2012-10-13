namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Base class for all request handlers whos content can be cached.
	/// </summary>
	public abstract class WebOutputRequestHandler : RequestHandler
	{
		#region Constructors
		#endregion
		#region Overrides of RequestHandler
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <returns>Returns the <see cref="WebResponse"/>.</returns>
		protected override WebResponse DoExecute(IMansionWebContext context)
		{
			// create the response
			var response = WebResponse.Create(context);

			// create an web output pipe, push it to the stack and allow implementors to process the request on it
			using (var outputPipe = new WebOutputPipe(response))
			using (context.OutputPipeStack.Push(outputPipe))
				DoExecute(context, outputPipe);

			// return the response
			return response;
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <param name="outputPipe">The <see cref="WebOutputPipe"/> to which the must should be written.</param>
		protected abstract void DoExecute(IMansionWebContext context, WebOutputPipe outputPipe);
		#endregion
	}
}