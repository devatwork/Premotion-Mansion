namespace Premotion.Mansion.Core.Patterns.Interpreting
{
	/// <summary>
	/// Represents an interpreter.
	/// </summary>
	/// <typeparam name="TInput">The type of input this interpreter interprets.</typeparam>
	/// <typeparam name="TInterpreted">The type of interpreted output.</typeparam>
	public interface IInterpreter<in TInput, out TInterpreted>
	{
		#region Interpret Methods
		/// <summary>
		/// Interprets the input.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		TInterpreted Interpret(IMansionContext context, TInput input);
		#endregion
	}
	/// <summary>
	/// Represents an interpreter.
	/// </summary>
	/// <typeparam name="TContext">The type of <see cref="IMansionContext"/>.</typeparam>
	/// <typeparam name="TInput">The type of input this interpreter interprets.</typeparam>
	/// <typeparam name="TInterpreted">The type of interpreted output.</typeparam>
	public interface IInterpreter<in TContext, in TInput, out TInterpreted> where TContext : class, IMansionContext
	{
		#region Interpret Methods
		/// <summary>
		/// Interprets the input.
		/// </summary>
		/// <param name="context">The <typeparamref name="TContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		TInterpreted Interpret(TContext context, TInput input);
		#endregion
	}
}