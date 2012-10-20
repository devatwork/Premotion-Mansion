using System;

namespace Premotion.Mansion.Core.Patterns.Pipes
{
	/// <summary>
	/// Represents a single stage within a <see cref="Pipeline{TDelegate}"/>.
	/// </summary>
	/// <typeparam name="TDelegate">The type of delegate which is executed by this stage.</typeparam>
	public class Stage<TDelegate> where TDelegate : class
	{
		#region Constructors
		private Stage(TDelegate @delegate)
		{
			// validate arguments
			if (@delegate == null)
				throw new ArgumentNullException("delegate");

			// set values
			this.@delegate = @delegate;
		}
		#endregion
		#region Operators
		/// <summary>
		/// Converts the given <paramref name="delegate"/> into a <see cref="Stage{TDelegate}"/>.
		/// </summary>
		/// <param name="delegate">The delegate executed in this stage.</param>
		/// <returns>Returns the constructed <see cref="Stage{TDelegate}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="delegate"/> is null.</exception>
		public static implicit operator Stage<TDelegate>(TDelegate @delegate)
		{
			// validate arguments
			if (@delegate == null)
				throw new ArgumentNullException("delegate");

			// return the pipeline stage
			return new Stage<TDelegate>(@delegate);
		}
		/// <summary>
		/// Gets the <typeparamref name="TDelegate"/> from the <paramref name="stage"/>.
		/// </summary>
		/// <param name="stage">The <see cref="Stage{TDelegate}"/>.</param>
		/// <returns>Returns the delegate.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="stage"/> is null.</exception>
		public static implicit operator TDelegate(Stage<TDelegate> stage)
		{
			// validate arguments
			if (stage == null)
				throw new ArgumentNullException("stage");

			// return the delegate
			return stage.@delegate;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the delegate of this stage.
		/// </summary>
		public TDelegate Delegate
		{
			get { return @delegate; }
		}
		#endregion
		#region Private Fields
		private readonly TDelegate @delegate;
		#endregion
	}
}