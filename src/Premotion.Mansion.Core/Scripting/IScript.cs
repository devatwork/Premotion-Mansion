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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		void Execute(IMansionContext context);
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <typeparam name="TResult">The result type.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the result of this script expression.</returns>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		TResult Execute<TResult>(IMansionContext context);
		#endregion
	}
}