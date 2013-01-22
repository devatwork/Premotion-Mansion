using System;
using System.Threading;

namespace Premotion.Mansion.Core.Patterns.Retry
{
	/// <summary>
	/// This retry strategy executes the given action N times before failing. Between each failure M milliseconds are waited.
	/// </summary>
	public class FixedIntervalStrategy : RetryStrategy
	{
		#region Constructors
		/// <summary>
		/// Constructs a fixed interval strategy.
		/// </summary>
		/// <param name="numberOfRetries">The number of times to attempt the action.</param>
		/// <param name="retryTimeout">The time waited between each attempt.</param>
		public FixedIntervalStrategy(int numberOfRetries, TimeSpan retryTimeout)
		{
			// validate arguments
			if (numberOfRetries < 0)
				throw new ArgumentOutOfRangeException("numberOfRetries");
			if (retryTimeout == null)
				throw new ArgumentNullException("retryTimeout");

			// set values
			this.numberOfRetries = numberOfRetries;
			this.retryTimeout = retryTimeout;
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the given <paramref name="action"/> using this retry strategy.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> which to execute.</param>
		protected override void DoExecute(Action action)
		{
			while (numberOfRetries > 0)
			{
				try
				{
					action();
					numberOfRetries = 0;
				}
				catch
				{
					numberOfRetries--;
					if (numberOfRetries == 0)
						throw;
					Thread.Sleep(retryTimeout);
				}
			}
		}
		#endregion
		#region Private Fields
		private readonly TimeSpan retryTimeout;
		private int numberOfRetries;
		#endregion
	}
}