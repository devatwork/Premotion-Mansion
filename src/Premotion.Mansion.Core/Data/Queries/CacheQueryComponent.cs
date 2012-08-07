namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Represents a limit <see cref="QueryComponent"/>.
	/// </summary>
	public class CacheQueryComponent : QueryComponent
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="CacheQueryComponent"/> from the given <paramref name="isEnabled"/>.
		/// </summary>
		/// <param name="isEnabled"></param>
		public CacheQueryComponent(bool isEnabled)
		{
			// set value
			this.isEnabled = isEnabled;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether the result of this query can be cached or not.
		/// </summary>
		public bool IsEnabled
		{
			get { return isEnabled; }
		}
		#endregion
		#region Private Fields
		private readonly bool isEnabled;
		#endregion
	}
}