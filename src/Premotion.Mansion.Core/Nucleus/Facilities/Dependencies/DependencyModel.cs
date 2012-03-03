using System.Collections.Generic;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Dependencies
{
	/// <summary>
	/// Defines the model of the dependencies for a <see cref="IServiceWithDependencies"/>.
	/// </summary>
	public class DependencyModel
	{
		#region Dependency Methods
		/// <summary>
		/// Adds a <see cref="Dependency"/> to this model.
		/// </summary>
		/// <typeparam name="TServiceContract">The contract of the dependend service.</typeparam>
		/// <returns>Returns the created dependency.</returns>
		public DependencyModel Add<TServiceContract>() where TServiceContract : IService
		{
			// add the dependency
			dependencies.Add(Dependency.Create<TServiceContract>());

			// return this for chaining);
			return this;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="Dependency"/>s of this service.
		/// </summary>
		public IEnumerable<Dependency> Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Private Fields
		private readonly List<Dependency> dependencies = new List<Dependency>();
		#endregion
	}
}