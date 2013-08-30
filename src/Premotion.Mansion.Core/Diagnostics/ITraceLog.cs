using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Diagnostics
{
	/// <summary>
	/// Tracks all trace messages during a given context.
	/// </summary>
	public interface ITraceLog
	{
		/// <summary>
		/// Writes an <paramref name="entry"/> to this trace log.
		/// </summary>
		/// <param name="entry">The <see cref="LogEntry"/> which to write to the log.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="entry"/> is null.</exception>
		void Write(LogEntry entry);
		/// <summary>
		/// Peeks all the <typeparamref name="TLogEntry"/> from this trace log.
		/// </summary>
		/// <typeparam name="TLogEntry">The type of <see cref="LogEntry"/> read from this trace log.</typeparam>
		/// <returns>Returns the matching log entries.</returns>
		IEnumerable<TLogEntry> Peek<TLogEntry>() where TLogEntry : LogEntry;
	}
}