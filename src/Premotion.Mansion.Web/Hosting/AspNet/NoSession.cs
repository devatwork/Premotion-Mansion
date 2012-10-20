using System;

namespace Premotion.Mansion.Web.Hosting.AspNet
{
	/// <summary>
	/// No session is available.
	/// </summary>
	public class NoSession : ISession
	{
		#region Implementation of ISessionProvider
		/// <summary>
		/// Gets a flag indicating wether this session is writeable or not.
		/// </summary>
		public bool IsWritable
		{
			get { return false; }
		}
		/// <summary>
		/// Gets a flag indicating wether this session is readable or not.
		/// </summary>
		public bool IsReadable
		{
			get { return false; }
		}
		/// <summary>
		/// Gets/Sets an object in this ession.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns the value.</returns>
		public object this[string key]
		{
			get { throw new NotSupportedException("There is no session"); }
			set { throw new NotSupportedException("There is no session"); }
		}
		/// <summary>
		/// Removes the given key from the session.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Remove(string key)
		{
			throw new NotSupportedException("There is no session");
		}
		#endregion
	}
}