namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Provides factory methods for creating <see cref="IRepository"/>s.
	/// </summary>
	public interface IRepositoryFactory
	{
		#region Factory Methods
		/// <summary>
		/// Creates a <see cref="IRepository"/> instance.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="arguments">The arguments which to use to create the <see cref="IRepository"/></param>
		/// <returns>Returns the created <see cref="IRepository"/>.</returns>
		IRepository Create(MansionContext context, IPropertyBag arguments);
		#endregion
	}
}