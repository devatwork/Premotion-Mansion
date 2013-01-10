using System;

namespace Premotion.Mansion.Core.Scripting
{
	/// <summary>
	/// This exception is thrown when a exception occurred while executing a <see cref="IScript"/>.
	/// </summary>
	public abstract class ScriptExecutionException : Exception
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="script"></param>
		/// <param name="innerException"></param>
		protected ScriptExecutionException(IScript script, Exception innerException) : base(string.Empty, innerException)
		{
			// validate arguments
			if (script == null)
				throw new ArgumentNullException("script");

			// set values
			Script = script;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the script which caused this exception.
		/// </summary>
		public IScript Script { get; private set; }
		#endregion
	}
}