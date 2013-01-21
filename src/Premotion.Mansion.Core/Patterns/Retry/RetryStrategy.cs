using System;

namespace Premotion.Mansion.Core.Patterns.Retry
{
	/// <summary>
	/// Represents a retry strategy.
	/// </summary>
	public abstract class RetryStrategy
	{
		/// <summary>
		/// Executes the given <paramref name="action"/> using this retry strategy.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> which to execute.</param>
		public void Execute(Action action)
		{
			// validate arguments
			if (action == null)
				throw new ArgumentNullException("action");

			//  invoke the template method
			DoExecute(action);
		}
		/// <summary>
		/// Executes the given <paramref name="action"/> using this retry strategy.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> which to execute.</param>
		protected abstract void DoExecute(Action action);
	}
}