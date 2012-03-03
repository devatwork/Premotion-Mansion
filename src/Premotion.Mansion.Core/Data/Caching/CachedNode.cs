using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Core.Data.Caching
{
	/// <summary>
	/// Implements <see cref="CachedObject{TObject}"/> for <see cref="Node"/>.
	/// </summary>
	public class CachedNode : CachedObject<Node>, IDependableCachedObject
	{
		#region Constructors
		/// <summary>
		/// Constructs a new cached object.
		/// </summary>
		/// <param name="obj">The object which to cache.</param>
		public CachedNode(Node obj) : base(obj)
		{
			Priority = Priority.Normal;

			// add the repository modified dependency
			Add(NodeCacheKeyFactory.RepositoryModifiedDependency);
		}
		#endregion
	}
}