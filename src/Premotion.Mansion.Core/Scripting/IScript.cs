namespace Premotion.Mansion.Core.Scripting
{
	/// <summary>
	/// Represents an executable script.
	/// </summary>
	public interface IScript
	{
		#region Execute Methods
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		void Execute(MansionContext context);
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <typeparam name="TResult">The result type.</typeparam>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the result of this script expression.</returns>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		TResult Execute<TResult>(MansionContext context);
		#endregion
	}
}