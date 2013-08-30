using System;

namespace Premotion.Mansion.Core.Diagnostics
{
	/// <summary>
	/// Holds the information of a single log entry.
	/// </summary>
	public abstract class LogEntry
	{
		/// <summary>
		/// Constructs a new log entry.
		/// </summary>
		/// <param name="source">The <see cref="Source"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		protected LogEntry(string source)
		{
			// validate arguments
			if (string.IsNullOrEmpty(source))
				throw new ArgumentNullException("source");

			// store the argument
			Source = source;
		}
		/// <summary>
		/// The subsystem which wrote this entry to the trace log.
		/// </summary>
		public string Source { get; private set; }
	}
}