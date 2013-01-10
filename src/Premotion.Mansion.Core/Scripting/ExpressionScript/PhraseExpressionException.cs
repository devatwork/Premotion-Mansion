using System;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Thrown when a <see cref="PhraseExpression"/> causes an <see cref="Exception"/>.
	/// </summary>
	[Serializable]
	public class PhraseExpressionException : ScriptExecutionException
	{
		#region Constructors
		/// <summary>
		/// Constructs a script function exception.
		/// </summary>
		/// <param name="sourceCode"></param>
		/// <param name="script"></param>
		/// <param name="innerException"></param>
		public PhraseExpressionException(string sourceCode, IScript script, Exception innerException) : base(script, innerException)
		{
			this.sourceCode = sourceCode;
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
			return string.Format("Expression '{0}' caused an exception", sourceCode);
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
		private readonly string sourceCode;
		#endregion
	}
}