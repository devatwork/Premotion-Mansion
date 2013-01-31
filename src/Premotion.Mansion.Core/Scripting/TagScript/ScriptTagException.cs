using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Thrown when an exception occured while executing a tag script.
	/// </summary>
	[Serializable]
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
		#region Overrides of Object
		/// <summary>
		/// Creates and returns a string representation of the current exception.
		/// </summary>
		/// <returns>
		/// A string representation of the current exception.
		/// </returns>
		/// <filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
		public override string ToString()
		{
			// get the most inner script tag
			var tag = scriptStackTrace.First();

			// format the exception message
			return string.Format("ScriptTag '{0}' at {1}:{2} caused an exception", Script.GetType(), tag.Info.Resource.Path.Paths.Last(), tag.Info.LineNumber);
		}
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
			get { return ToString(); }
		}
		#endregion
		#region Private Fields
		private readonly ICollection<ScriptTag> scriptStackTrace = new List<ScriptTag>();
		#endregion
	}
}