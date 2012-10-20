using System;
using System.Linq;
using Premotion.Mansion.Core.Patterns.Pipes;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Represents the before pipeline which is executed before the request is handled.
	/// </summary>
	public class BeforePipeline : Pipeline<Func<IMansionWebContext, WebResponse>>
	{
		#region Execute Methods
		/// <summary>
		/// Executes the <see cref="Pipeline{TDelegate}.Stages"/> within this pipeline.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns true when one of the stages already handled the request, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Throw if <paramref name="context"/> is null.</exception>
		public WebResponse Execute(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// loop over all the stages in this pipeline and execute them until one returns false
			return Stages.Select(candidate => candidate.Delegate(context)).FirstOrDefault();
		}
		#endregion
		#region Operators
		/// <summary>
		/// Adds the given <paramref name="delegate"/> to the <paramref name="pipeline"/>.
		/// </summary>
		/// <param name="pipeline">The <see cref="AfterPipeline"/> to which to add the stage.</param>
		/// <param name="delegate">The delegate executed within the stage.</param>
		/// <returns>Returns the <paramref name="pipeline"/>.</returns>
		public static BeforePipeline operator +(BeforePipeline pipeline, Func<IMansionWebContext, WebResponse> @delegate)
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