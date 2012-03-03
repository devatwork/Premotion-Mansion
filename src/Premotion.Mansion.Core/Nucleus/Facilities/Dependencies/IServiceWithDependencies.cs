namespace Premotion.Mansion.Core.Nucleus.Facilities.Dependencies
{
	///<summary>
	/// Services implementing this inteface have dependencies on other <see cref="IService"/>s.
	///</summary>
	public interface IServiceWithDependencies : IService
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		DependencyModel Dependencies { get; }
		#endregion
	}
}