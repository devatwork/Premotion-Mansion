using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Thrown when an exception occured while executing a tag script.
	/// </summary>
	public class ScriptTagException : ScriptExecutionException
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="innerException"></param>
		public ScriptTagException(ScriptTag tag, Exception innerException) : base(tag, innerException)
		{
			// validate arguments
			if (tag == null)
				throw new ArgumentNullException("tag");

			// add to trace
			ScriptStackTrace.Add(tag);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the stacktrace.
		/// </summary>
		public ICollection<ScriptTag> ScriptStackTrace
		{
			get { return scriptStackTrace; }
		}
		#endregion
		#region Private Fields
		private readonly ICollection<ScriptTag> scriptStackTrace = new List<ScriptTag>();
		#endregion
	}
}