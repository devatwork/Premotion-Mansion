using System;

namespace Premotion.Mansion.Core.Patterns.Retry
{
	/// <summary>
	/// Provides extension methods for retrying the connection.
	/// </summary>
	public static class RetryExtensions
	{
		#region Retry Methods
		/// <summary>
		/// Executes the <paramref name="action"/> using the given retry <paramref name="strategy"/>.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> which to execute.</param>
		/// <param name="strategy">The <see cref="RetryStrategy"/>.</param>
		public static void Retry(this Action action, RetryStrategy strategy)
		{
			// validate arguments
			if (action == null)
				throw new ArgumentNullException("action");
			if (strategy == null)
				throw new ArgumentNullException("strategy");

			// invoke the method using the retry strategy
			strategy.Execute(action);
		}
		#endregion
	}
}