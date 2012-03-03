namespace Premotion.Mansion.Core.Caching
{
	/// <summary>
	/// Indicates the propiority of an item in the cache.
	/// </summary>
	public enum Priority
	{
		/// <summary>
		/// Indicates that the item is not removable.
		/// </summary>
		NotRemovable,
		/// <summary>
		/// Indicates that the item is last to be removed.
		/// </summary>
		Normal,
		/// <summary>
		/// Indicates that the item is removable.
		/// </summary>
		High,
		/// <summary>
		/// Indicates that the item is first to be removed.
		/// </summary>
		Low,
	}
}