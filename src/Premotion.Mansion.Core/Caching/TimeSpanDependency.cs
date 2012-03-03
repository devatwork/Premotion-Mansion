using System;

namespace Premotion.Mansion.Core.Caching
{
	/// <summary>
	/// Implements <see cref="CacheDependency"/> using a <see cref="TimeSpan"/>.
	/// </summary>
	public class TimeSpanDependency : CacheDependency
	{
		#region Constructors
		/// <summary>
		/// Constructs a timespan cache dependency.
		/// </summary>
		/// <param name="timeSpan">The timespan.</param>
		public TimeSpanDependency(TimeSpan timeSpan)
		{
			// validate arguments
			if (timeSpan.TotalMilliseconds < 0)
				throw new ArgumentOutOfRangeException("timeSpan");

			// set value
			this.timeSpan = timeSpan;
		}
		#endregion
		#region ToString Methods
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return timeSpan.ToString();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the timespan.
		/// </summary>
		public TimeSpan Timespan
		{
			get { return timeSpan; }
		}
		#endregion
		#region Private Fields
		private readonly TimeSpan timeSpan;
		#endregion
	}
}