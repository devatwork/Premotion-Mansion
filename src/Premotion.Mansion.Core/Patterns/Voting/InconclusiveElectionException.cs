using System;
using System.Collections.Generic;
using System.Text;

namespace Premotion.Mansion.Core.Patterns.Voting
{
	/// <summary>
	/// Thrown when there is no winner in the election.
	/// </summary>
	/// <typeparam name="TCandidate">The type of voter.</typeparam>
	/// <typeparam name="TSubject">The type of subject on which to vote.</typeparam>
	public class InconclusiveElectionException<TCandidate, TSubject> : Exception where TCandidate : ICandidate<TSubject>
	{
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="InconclusiveElectionException{TCandidate,TSubject}"/>.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="candidates">The <typeparamref name="TCandidate"/>s</param>
		/// <param name="subject">The <typeparamref name="TSubject"/>.</param>
		public InconclusiveElectionException(string message, IEnumerable<TCandidate> candidates, TSubject subject) : base(message)
		{
			// loop over the request parameters
			var builder = new StringBuilder();
			foreach (var candidate in candidates)
				builder.AppendFormat(" - {0}{1}", candidate.GetType(), Environment.NewLine);
			if (builder.Length == 0)
				builder.Append(" - None");

			Details = string.Format("Message: {1}{0}{0}Subject:{0} - Type: {2}{0} - Value: {3}{0}{0}Candidates:{0}{4}", Environment.NewLine, message, subject.GetType(), subject, builder);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the request details.
		/// </summary>
		public string Details { get; private set; }
		#endregion
		#region Overrides of Exception
		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <returns>
		/// The error message that explains the reason for the exception, or an empty string("").
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override string Message
		{
			get { return Details; }
		}
		#endregion
	}
}