using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Patterns.Pipes
{
	/// <summary>
	/// Represents a processing pipeline, which is a list of pipeline <see cref="Stage{TDelegate}"/>s.
	/// </summary>
	/// <typeparam name="TDelegate">The type of delegate which is executed by the stages in this pipeline.</typeparam>
	public abstract class Pipeline<TDelegate> where TDelegate : class
	{
		#region Add Methods
		/// <summary>
		/// Adds the <paramref name="stage"/> to the begin of this pipeline.
		/// </summary>
		/// <param name="stage">The <see cref="Stage{TDelegate}"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="stage"/> is null.</exception>
		public void AddStageToBeginOfPipeline(Stage<TDelegate> stage)
		{
			// validate arguments
			if (stage == null)
				throw new ArgumentNullException("stage");

			// add the list of stages
			stages.Insert(0, stage);
		}
		/// <summary>
		/// Adds the <paramref name="delegate"/> as a <see cref="Stage{TDelegate}"/>to the begin of this pipeline.
		/// </summary>
		/// <param name="delegate">The <typeparamref name="TDelegate"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="delegate"/> is null.</exception>
		public void AddStageToBeginOfPipeline(TDelegate @delegate)
		{
			// validate arguments
			if (@delegate == null)
				throw new ArgumentNullException("delegate");

			// add the list of stages
			AddStageToBeginOfPipeline((Stage<TDelegate>) @delegate);
		}
		/// <summary>
		/// Adds the <paramref name="stage"/> to the end of this pipeline.
		/// </summary>
		/// <param name="stage">The <see cref="Stage{TDelegate}"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="stage"/> is null.</exception>
		public void AddStageToEndOfPipeline(Stage<TDelegate> stage)
		{
			// validate arguments
			if (stage == null)
				throw new ArgumentNullException("stage");

			// add the list of stages
			stages.Add(stage);
		}
		/// <summary>
		/// Adds the <paramref name="delegate"/> as a <see cref="Stage{TDelegate}"/>to the end of this pipeline.
		/// </summary>
		/// <param name="delegate">The <typeparamref name="TDelegate"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="delegate"/> is null.</exception>
		public void AddStageToEndOfPipeline(TDelegate @delegate)
		{
			// validate arguments
			if (@delegate == null)
				throw new ArgumentNullException("delegate");

			// add the list of stages
			AddStageToEndOfPipeline((Stage<TDelegate>) @delegate);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets all the <see cref="Stage{TDTDelegate}"/> within this pipeline.
		/// </summary>
		protected IEnumerable<Stage<TDelegate>> Stages
		{
			get { return stages; }
		}
		#endregion
		#region Private Fields
		private readonly List<Stage<TDelegate>> stages = new List<Stage<TDelegate>>();
		#endregion
	}
}