using System;
using System.Web;

namespace Premotion.Mansion.Web.Hosting.AspNet
{
	/// <summary>
	/// Wraps the <see cref="HttpSessionStateBase"/>.
	/// </summary>
	public class AspNetReadOnlySession : ISession
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="session">The <see cref="HttpSessionStateBase"/>.</param>
		public AspNetReadOnlySession(HttpSessionStateBase session)
		{
			// validate arguments
			if (session == null)
				throw new ArgumentNullException("session");

			// set variable
			this.session = session;
		}
		#endregion
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
			get { return true; }
		}
		/// <summary>
		/// Gets/Sets an object in this ession.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns the value.</returns>
		public object this[string key]
		{
			get { return session[key]; }
			set { throw new NotSupportedException("This session is read-only"); }
		}
		/// <summary>
		/// Removes the given key from the session.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Remove(string key)
		{
			throw new NotSupportedException("This session is read-only");
		}
		#endregion
		#region Private Fields
		private readonly HttpSessionStateBase session;
		#endregion
	}
}