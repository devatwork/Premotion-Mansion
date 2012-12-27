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
}