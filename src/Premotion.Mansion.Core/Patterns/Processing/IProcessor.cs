namespace Premotion.Mansion.Core.Patterns.Processing
{
	/// <summary>
	/// Represents a processor.
	/// </summary>
	/// <typeparam name="TInput">The type of input which to process.</typeparam>
	public interface IProcessor<in TInput>
	{
		#region Process Methods
		/// <summary>
		/// Processes the <paramref name="input"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to process.</param>
		void Process(IMansionContext context, TInput input);
		#endregion
	}
	/// <summary>
	/// Represents a processor.
	/// </summary>
	/// <typeparam name="TContext">The type of <see cref="IMansionContext"/>.</typeparam>
	/// <typeparam name="TInput">The type of input which to process.</typeparam>
	public interface IProcessor<in TContext, in TInput> where TContext : class, IMansionContext
	{
		#region Process Methods
		/// <summary>
		/// Processes the <paramref name="input"/>.
		/// </summary>
		/// <param name="context">The <typeparamref name="TContext"/>.</param>
		/// <param name="input">The input which to process.</param>
		void Process(TContext context, TInput input);
		#endregion
	}
	/// <summary>
	/// Represents a processor.
	/// </summary>
	/// <typeparam name="TContext">The type of <see cref="IMansionContext"/>.</typeparam>
	/// <typeparam name="TInput">The type of input which to process.</typeparam>
	/// <typeparam name="TOutput">The type of output on which to operate.</typeparam>
	public interface IProcessor<in TContext, in TInput, in TOutput> where TContext : class, IMansionContext
	{
		#region Process Methods
		/// <summary>
		/// Processes the <paramref name="input"/>.
		/// </summary>
		/// <param name="context">The <typeparamref name="TContext"/>.</param>
		/// <param name="input">The input which to process.</param>
		/// <param name="output">The output on which to operate.</param>
		void Process(TContext context, TInput input, TOutput output);
		#endregion
	}
}