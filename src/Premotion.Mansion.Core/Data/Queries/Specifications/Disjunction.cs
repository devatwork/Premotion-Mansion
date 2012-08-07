namespace Premotion.Mansion.Core.Data.Queries.Specifications
{
	/// <summary>
	/// Combines all the <see cref="Specification"/>s within using the logical OR operator.
	/// </summary>
	public class Disjunction : CompositeSpecification
	{
		#region Constructors
		/// <summary>
		/// Constructs a disjunction.
		/// </summary>
		public Disjunction() : base("or")
		{
		}
		#endregion
	}
}