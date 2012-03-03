namespace Premotion.Mansion.Core.Patterns.Voting
{
	/// <summary>
	/// Represents a single vote.
	/// </summary>
	public class VoteResult
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="strength"></param>
		private VoteResult(byte strength)
		{
			// set values
			this.strength = strength;
		}
		#endregion
		#region Votes
		/// <summary>
		/// Value for highest interest.
		/// </summary>
		public static VoteResult HighestInterest = new VoteResult(byte.MaxValue - 25);
		/// <summary>
		/// Value for high interest.
		/// </summary>
		public static VoteResult HighInterest = new VoteResult(byte.MaxValue - 50);
		/// <summary>
		/// Value for low interest.
		/// </summary>
		public static VoteResult LowInterest = new VoteResult(byte.MinValue + 50);
		/// <summary>
		/// Value for lowest interest.
		/// </summary>
		public static VoteResult LowestInterest = new VoteResult(byte.MinValue + 25);
		/// <summary>
		/// Value for refraining.
		/// </summary>
		public static VoteResult Refrain = new VoteResult(byte.MinValue);
		/// <summary>
		/// Value for medium interest.
		/// </summary>
		public static VoteResult MediumInterest = new VoteResult(byte.MinValue + 125);
		/// <summary>
		/// Value for veto.
		/// </summary>
		public static VoteResult Veto = new VoteResult(byte.MaxValue);
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates a vote with a custom interest.
		/// </summary>
		/// <param name="strength">The strength of the vote.</param>
		/// <returns>Returns the created vote.</returns>
		public static VoteResult Create(byte strength)
		{
			return new VoteResult(strength);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the strength of this vote.
		/// </summary>
		public byte Strength
		{
			get { return strength; }
		}
		#endregion
		#region Private Fields
		private readonly byte strength;
		#endregion
	}
}