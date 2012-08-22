using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Caching
{
	/// <summary>
	/// Represents a cached object.
	/// </summary>
	/// <typeparam name="TObject">The type of object wrapped in this container.</typeparam>
	public class CachedObject<TObject>
	{
		#region Constructors
		/// <summary>
		/// Constructs a new cached object.
		/// </summary>
		/// <param name="obj">The object which to cache.</param>
		/// <param name="priority">The <see cref="Priority"/> of this cache entry..</param>
		/// <param name="isCachable">The flag indicating whether the object is cacheable by default or not.</param>
		public CachedObject(TObject obj, Priority priority = Priority.Normal, bool isCachable = true)
		{
			Object = obj;
			Priority = priority;
			IsCachable = isCachable;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates a instance of <see cref="CachedObject{TObject}"/>.
		/// </summary>
		/// <param name="obj">The object which is cached.</param>
		/// <returns>Returns the cached object.</returns>
		public static CachedObject<TObject> Create(TObject obj)
		{
			return new CachedObject<TObject>(obj);
		}
		#endregion
		#region Dependency Methods
		/// <summary>
		/// Adds a dependency to this cached object.
		/// </summary>
		/// <param name="dependency">The dependency to add.</param>
		public void Add(CacheDependency dependency)
		{
			// validate arguments
			if (dependency == null)
				throw new ArgumentNullException("dependency");
			dependencies.Add(dependency);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the dependecies of this object.
		/// </summary>
		public IEnumerable<CacheDependency> Dependencies
		{
			get { return dependencies; }
		}
		/// <summary>
		/// Gets the wrapped object.
		/// </summary>
		public TObject Object { get; private set; }
		/// <summary>
		/// Gets the priority of this cache object.
		/// </summary>
		public Priority Priority { get; set; }
		/// <summary>
		/// Gets/Sets a flag indicating whether caching is enabled for this object.
		/// </summary>
		public bool IsCachable { get; set; }
		#endregion
		#region Private Fields
		private readonly List<CacheDependency> dependencies = new List<CacheDependency>();
		#endregion
	}
}