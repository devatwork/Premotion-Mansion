using System;
using Premotion.Mansion.Core.Patterns.Pipes;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Represents the after pipeline which is executed after the request is handled.
	/// </summary>
	public class AfterPipeline : Pipeline<Action<IMansionWebContext, WebResponse>>
	{
		#region Execute Methods
		/// <summary>
		/// Executes the <see cref="Pipeline{TDelegate}.Stages"/> within this pipeline.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="response">The <see cref="WebResponse"/>.</param>
		/// <exception cref="ArgumentNullException">Throw if <paramref name="context"/> or <paramref name="response"/> is null.</exception>
		public void Execute(IMansionWebContext context, WebResponse response)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (response == null)
				throw new ArgumentNullException("response");

			// loop over all the stages in this pipeline and execute them
			foreach (var stage in Stages)
				stage.Delegate(context, response);
		}
		#endregion
		#region Operators
		/// <summary>
		/// Adds the given <paramref name="delegate"/> to the <paramref name="pipeline"/>.
		/// </summary>
		/// <param name="pipeline">The <see cref="AfterPipeline"/> to which to add the stage.</param>
		/// <param name="delegate">The delegate executed within the stage.</param>
		/// <returns>Returns the <paramref name="pipeline"/>.</returns>
		public static AfterPipeline operator +(AfterPipeline pipeline, Action<IMansionWebContext, WebResponse> @delegate)
		{
			// validate arguments
			if (pipeline == null)
				throw new ArgumentNullException("pipeline");
			if (@delegate == null)
				throw new ArgumentNullException("delegate");

			// add the stage to the pipeline
			pipeline.AddStageToEndOfPipeline(@delegate);
			return pipeline;
		}
		#endregion
	}
}