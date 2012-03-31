using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Patterns.Interpreting
{
	/// <summary>
	/// Represents an voting interpreter.
	/// </summary>
	/// <typeparam name="TInput">The type of input this interpreter interprets.</typeparam>
	/// <typeparam name="TInterpreted">The type of interpreted output.</typeparam>
	public interface IVotingInterpreter<in TInput, out TInterpreted> : IInterpreter<TInput, TInterpreted>, ICandidate<TInput>
	{
	}
	/// <summary>
	/// Represents an voting interpreter.
	/// </summary>
	/// <typeparam name="TContext">The type of <see cref="IMansionContext"/>.</typeparam>
	/// <typeparam name="TInput">The type of input this interpreter interprets.</typeparam>
	/// <typeparam name="TInterpreted">The type of interpreted output.</typeparam>
	public interface IVotingInterpreter<in TContext, in TInput, out TInterpreted> : IInterpreter<TContext, TInput, TInterpreted>, ICandidate<TContext, TInput> where TContext : class, IMansionContext
	{
	}
}