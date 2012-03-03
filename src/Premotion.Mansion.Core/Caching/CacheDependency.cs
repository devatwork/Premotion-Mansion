namespace Premotion.Mansion.Core.Caching
{
	/// <summary>
	/// Represents a cache dependency.
	/// </summary>
	public abstract class CacheDependency
	{
		#region ToString Methods
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public abstract override string ToString();
		#endregion
	}
}