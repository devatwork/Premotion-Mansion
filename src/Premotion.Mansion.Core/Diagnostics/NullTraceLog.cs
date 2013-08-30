using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Diagnostics
{
	/// <summary>
	/// Ignores all the writes to the log.
	/// </summary>
	public class NullTraceLog : ITraceLog
	{
		/// <summary>
		/// Holds the instance.
		/// </summary>
		public static ITraceLog Instance = new NullTraceLog();
		/// <summary>
		/// Writes an <paramref name="entry"/> to this trace log.
		/// </summary>
		/// <param name="entry">The <see cref="LogEntry"/> which to write to the log.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="entry"/> is null.</exception>
		public void Write(LogEntry entry)
		{
			// do nothing
		}
		/// <summary>
		/// Peeks all the <typeparamref name="TLogEntry"/> from this trace log.
		/// </summary>
		/// <typeparam name="TLogEntry">The type of <see cref="LogEntry"/> read from this trace log.</typeparam>
		/// <returns>Returns the matching log entries.</returns>
		public IEnumerable<TLogEntry> Peek<TLogEntry>() where TLogEntry : LogEntry
		{
			return Enumerable.Empty<TLogEntry>();
		}
	}
}