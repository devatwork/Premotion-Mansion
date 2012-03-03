namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Defines the public interface for <see cref="IContext"/> which are aware of <see cref="INucleus"/>.
	/// </summary>
	public interface INucleusAwareContext : IContext
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="INucleus"/> associated with this context.
		/// </summary>
		INucleus Nucleus { get; }
		#endregion
	}
}