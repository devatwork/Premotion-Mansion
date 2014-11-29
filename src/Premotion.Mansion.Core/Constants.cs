using System;
using System.Configuration;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// 
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Indicating whether the application is live (true) or in staging mode (false).
		/// </summary>
		public static bool ApplicationIsLive = Convert.ToBoolean(ConfigurationManager.AppSettings["APPLICATION_IS_LIVE"]);
		
	}
}