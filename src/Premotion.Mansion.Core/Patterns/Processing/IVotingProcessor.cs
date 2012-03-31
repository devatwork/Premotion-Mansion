using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Patterns.Processing
{
	/// <summary>
	/// Represents a voting processor.
	/// </summary>
	/// <typeparam name="TInput">The type of input which to process.</typeparam>
	public interface IVotingProcessor<in TInput> : IProcessor<TInput>, ICandidate<TInput>
	{
	}
	/// <summary>
	/// Represents a voting processor.
	/// </summary>
	/// <typeparam name="TContext">The type of <see cref="IMansionContext"/>.</typeparam>
	/// <typeparam name="TInput">The type of input which to process.</typeparam>
	public interface IVotingProcessor<in TContext, in TInput> : IProcessor<TContext, TInput>, ICandidate<TContext, TInput> where TContext : class, IMansionContext
	{
	}
}