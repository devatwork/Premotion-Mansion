namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Interfaces for accessing session information.
	/// </summary>
	public interface ISession
	{
		/// <summary>
		/// Gets a flag indicating wether this session is writeable or not.
		/// </summary>
		bool IsWritable { get; }
		/// <summary>
		/// Gets a flag indicating wether this session is readable or not.
		/// </summary>
		bool IsReadable { get; }
		/// <summary>
		/// Gets/Sets an object in this ession.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns the value.</returns>
		object this[string key] { get; set; }
		/// <summary>
		/// Removes the given key from the session.
		/// </summary>
		/// <param name="key">The key.</param>
		void Remove(string key);
	}
}