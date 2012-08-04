namespace Premotion.Mansion.Core.Data.Specifications
{
	/// <summary>
	/// Combines all the <see cref="Specification"/>s within using the logical AND operator.
	/// </summary>
	public class Conjunction : CompositeSpecification
	{
		#region Constructors
		/// <summary>
		/// Constructs a disjunction.
		/// </summary>
		public Conjunction() : base("and")
		{
		}
		#endregion
	}
}