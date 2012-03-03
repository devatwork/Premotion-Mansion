using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Core.Data.Caching
{
	/// <summary>
	/// Implements <see cref="CachedObject{TObject}"/> for <see cref="Nodeset"/>.
	/// </summary>
	public class CachedNodeset : CachedObject<Nodeset>, IDependableCachedObject
	{
		#region Constructors
		/// <summary>
		/// Constructs a new cached object.
		/// </summary>
		/// <param name="obj">The object which to cache.</param>
		public CachedNodeset(Nodeset obj) : base(obj)
		{
			Priority = Priority.Normal;

			// add the repository modified dependency
			Add(NodeCacheKeyFactory.RepositoryModifiedDependency);
		}
		#endregion
	}
}