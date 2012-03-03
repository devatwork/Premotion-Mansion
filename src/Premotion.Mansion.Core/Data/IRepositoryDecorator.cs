namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Marker interface for <see cref="IRepositoryDecorator"/> decorators.
	/// </summary>
	public interface IRepositoryDecorator : IRepository
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="IRepository"/> being decorated.
		/// </summary>
		IRepository DecoratedRepository { get; }
		#endregion
	}
}